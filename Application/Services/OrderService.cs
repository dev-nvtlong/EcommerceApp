using AutoMapper;
using EcommerceApp.Application.DTOs.Order;
using EcommerceApp.Application.Interfaces.Repositories;
using EcommerceApp.Application.Interfaces.Services;
using EcommerceApp.Models;

namespace EcommerceApp.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IProductRepository productRepository, IMapper mapper, INotificationService notificationService)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<OrderDto> CreateOrderAsync(int userId, CreateOrderDto createOrderDto)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null || cart.Items == null || !cart.Items.Any())
            {
                throw new Exception("Giỏ hàng trống");
            }

            var totalAmount = cart.Items.Sum(item => item.Quantity * item.Product.Price);

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                PaymentMethod = createOrderDto.PaymentMethod,
                Status = Enums.OrderStatus.Pending,
                TotalAmount = totalAmount,
                Details = cart.Items.Select(item => new OrderDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                }).ToList()
            };

            var createdOrder = await _orderRepository.CreateAsync(order);

            // Clear Cart
            foreach (var item in cart.Items.ToList())
            {
                await _cartRepository.RemoveCartItemAsync(item);
            }
            await _cartRepository.SaveAsync();

            // Create Notification for Admin
            await _notificationService.CreateNotificationAsync(
                "Đơn hàng mới", 
                $"Bạn có đơn hàng mới #{createdOrder.ID} với tổng tiền {createdOrder.TotalAmount.ToString("N0")}đ", 
                Enums.NotificationType.NewOrder,
                $"/Admin/Order/Details/{createdOrder.ID}"
            );

            return _mapper.Map<OrderDto>(createdOrder);
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<List<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetAllByUserIdAsync(userId);
            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllWithUserAsync();
            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task UpdateOrderStatusAsync(int orderId, EcommerceApp.Enums.OrderStatus status)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order != null)
            {
                // Subtract stock when order is completed
                if (status == Enums.OrderStatus.Completed && order.Status != Enums.OrderStatus.Completed)
                {
                    if (order.Details != null)
                    {
                        foreach (var detail in order.Details)
                        {
                            var product = await _productRepository.GetByIdAsync(detail.ProductId);
                            if (product != null)
                            {
                                product.StockQuantity -= detail.Quantity;
                                product.SoldCount = (product.SoldCount ?? 0) + detail.Quantity;
                                await _productRepository.UpdateAsync(product);
                                
                                if (product.StockQuantity < 5)
                                {
                                    await _notificationService.CreateNotificationAsync(
                                        "Cảnh báo tồn kho", 
                                        $"Sản phẩm '{product.Name}' sắp hết hàng (vừa giảm xuống {product.StockQuantity} cây).", 
                                        Enums.NotificationType.LowStock,
                                        $"/Admin/Product/Index"
                                    );
                                }
                            }
                        }
                    }
                }

                order.Status = status;
                await _orderRepository.UpdateAsync(order);
            }
        }
        public async Task UpdateOrderAsync(int orderId, OrderDto orderDto)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order != null)
            {
                order.ShipName = orderDto.ShipName;
                order.ShipPhone = orderDto.ShipPhone;
                order.ShipAddress = orderDto.ShipAddress;
                await _orderRepository.UpdateAsync(order);
            }
        }
    }
}

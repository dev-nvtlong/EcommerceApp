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

        public async Task<OrderDto> CreateOrderAsync(Guid userId, CreateOrderDto createOrderDto)
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
                ShipName = createOrderDto.ShipName,
                ShipAddress = createOrderDto.ShipAddress,
                ShipPhone = createOrderDto.ShipPhone,
                Details = cart.Items.Select(item => new OrderDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                }).ToList()
            };

            var createdOrder = await _orderRepository.CreateAsync(order);
            
            // Subtract stock and update SoldCount immediately on order creation for reservation
            foreach (var item in cart.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    if (product.StockQuantity < item.Quantity)
                    {
                        throw new Exception($"Sản phẩm '{product.Name}' hiện chỉ còn {product.StockQuantity} cây, không đủ để đặt hàng.");
                    }
                    product.StockQuantity -= item.Quantity;
                    product.SoldCount = (product.SoldCount ?? 0) + item.Quantity;
                    await _productRepository.UpdateAsync(product);
                    
                    if (product.StockQuantity < 5)
                    {
                        await _notificationService.CreateNotificationAsync(
                            "Cảnh báo tồn kho", 
                            $"Sản phẩm '{product.Name}' sắp hết hàng (còn {product.StockQuantity} cây).", 
                            Enums.NotificationType.LowStock,
                            $"/Admin/Product/Index"
                        );
                    }
                }
            }

            // Clear Cart
            foreach (var item in cart.Items.ToList())
            {
                await _cartRepository.RemoveCartItemAsync(item);
            }
            await _cartRepository.SaveAsync();

            // Create Notification for Admin
            await _notificationService.CreateNotificationAsync(
                "Đơn hàng mới", 
                $"Bạn có đơn hàng mới #{createdOrder.Id} với tổng tiền {createdOrder.TotalAmount.ToString("N0")}đ", 
                Enums.NotificationType.NewOrder,
                $"/Admin/Order/Details/{createdOrder.Id}"
            );

            return _mapper.Map<OrderDto>(createdOrder);
        }

        public async Task<OrderDto?> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<List<OrderDto>> GetOrdersByUserIdAsync(Guid userId)
        {
            var orders = await _orderRepository.GetAllByUserIdAsync(userId);
            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllWithUserAsync();
            return _mapper.Map<List<OrderDto>>(orders);
        }

        public async Task UpdateOrderStatusAsync(Guid orderId, EcommerceApp.Enums.OrderStatus status)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order != null)
            {
                // If Cancelled and wasn't already Cancelled, return stock
                if (status == Enums.OrderStatus.Cancelled && order.Status != Enums.OrderStatus.Cancelled)
                {
                    if (order.Details != null)
                    {
                        foreach (var detail in order.Details)
                        {
                            var product = await _productRepository.GetByIdAsync(detail.ProductId);
                            if (product != null)
                            {
                                product.StockQuantity += detail.Quantity;
                                product.SoldCount = Math.Max(0, (product.SoldCount ?? 0) - detail.Quantity);
                                await _productRepository.UpdateAsync(product);
                            }
                        }
                    }
                }
                // (Optional) If transitioning from Cancelled back to any active status:
                else if (order.Status == Enums.OrderStatus.Cancelled && status != Enums.OrderStatus.Cancelled)
                {
                    if (order.Details != null)
                    {
                        foreach (var detail in order.Details)
                        {
                            var product = await _productRepository.GetByIdAsync(detail.ProductId);
                            if (product != null)
                            {
                                if(product.StockQuantity < detail.Quantity)
                                {
                                     throw new Exception($"Không thể khôi phục đơn hàng. Sản phẩm '{product.Name}' không đủ tồn kho.");
                                }
                                product.StockQuantity -= detail.Quantity;
                                product.SoldCount = (product.SoldCount ?? 0) + detail.Quantity;
                                await _productRepository.UpdateAsync(product);
                            }
                        }
                    }
                }

                order.Status = status;
                await _orderRepository.UpdateAsync(order);
            }
        }
        public async Task UpdateOrderAsync(Guid orderId, OrderDto orderDto)
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

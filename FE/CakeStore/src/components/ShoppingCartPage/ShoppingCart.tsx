import React from "react";
import "./ShoppingCart.css";

function ShoppingCart() {
  const cartItems = [
    {
      id: 1,
      name: "Chocolate Cake",
      price: 15.99,
      quantity: 2,
      image: "https://via.placeholder.com/100?text=Chocolate+Cake",
    },
    {
      id: 2,
      name: "Strawberry Cheesecake",
      price: 12.99,
      quantity: 1,
      image: "https://via.placeholder.com/100?text=Strawberry+Cheesecake",
    },
    {
      id: 3,
      name: "Vanilla Cupcake",
      price: 3.99,
      quantity: 4,
      image: "https://via.placeholder.com/100?text=Vanilla+Cupcake",
    },
  ];

  const calculateTotal = () => {
    return cartItems
      .reduce((total, item) => total + item.price * item.quantity, 0)
      .toFixed(2);
  };

  return (
    <div className="shopping-cart">
      <h1>Shopping Cart</h1>
      <ul>
        {cartItems.map((item) => (
          <li key={item.id}>
            <img src={item.image} alt={item.name} className="product-image" />
            <div className="product-info">
              <span className="product-name">{item.name}</span>
              <span className="product-quantity">x {item.quantity}</span>
              <span className="product-price">${item.price}</span>
            </div>
            <button className="remove-button">Remove</button>
          </li>
        ))}
      </ul>
      <p>Total: ${calculateTotal()}</p>
      <button className="checkout-button">Checkout</button>
    </div>
  );
}

export default ShoppingCart;

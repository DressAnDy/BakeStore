import React from "react";
import "./MenuPage.css";
const MenuPage = () => {
  return (
    <div className="container">
      <header className="header-bar">
        <nav className="nav">
          <ul>
            <li>
              <a href="#">Home</a>
            </li>
            <li>
              <a href="#">Cart</a>
            </li>
            <li>
              <a href="#">Login</a>
            </li>
          </ul>
        </nav>
      </header>
      <div className="body-content">
        <div className="category-section">
          <h2>Categories</h2>
          <ul>
            <li>Chocolate</li>
            <li>Vanilla</li>
            <li>Red Velvet</li>
          </ul>
        </div>
        <div className="menu-section">
          <h2>Menu</h2>
          <div className="search-bar">
            <input type="search" placeholder="Search Cakes" />
          </div>
          <div className="cake-list">
            <div className="cake-row">
              <div className="cake">
                <h3>Chocolate Cake</h3>
                <p>Price: $10.99</p>
                <button>Add to Cart</button>
              </div>
              <div className="cake">
                <h3>Vanilla Cake</h3>
                <p>Price: $9.99</p>
                <button>Add to Cart</button>
              </div>
              <div className="cake">
                <h3>Red Velvet Cake</h3>
                <p>Price: $12.99</p>
                <button>Add to Cart</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default MenuPage;

import React from "react";
import "./HomePage.css";

function HomePage() {
  return (
    <div className="homepage">
      <div className="header">
        <h1>Cake Store</h1>
        <nav>
          <ul>
            <li>
              <a href="/">Home</a>
            </li>
            <li>
              <a href="/cakes">Cakes</a>
            </li>
            <li>
              <a href="/about">Menu</a>
            </li>
            <li>
              <a href="/contact">Contact Us</a>
            </li>
            <li>
              <a href="/login">Login/Register</a>
            </li>
          </ul>
        </nav>
      </div>

      <div className="hero">
        <img src="https://example.com/cake-hero.jpg" alt="Cake Hero" />
        <div className="hero-content">
          <h2>Indulge in the Sweet Life with Cake Store</h2>
          <p>
            Explore our wide selection of artisanal cakes, crafted with love and
            care
          </p>
          <button>Explore Our Cakes</button>
        </div>
      </div>

      <div className="featured-cakes">
        <h2>Signature Cakes</h2>
        <div className="cakes-container">
          {["Cake 1", "Cake 2", "Cake 3"].map((cake, index) => (
            <div className="cake-item" key={index}>
              <img
                src={`https://example.com/cake${index + 1}.jpg`}
                alt={cake}
              />
              <h3>{cake}</h3>
              <p>${(index + 1) * 5 + 5}.99</p>
              <p>
                Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed sit
                amet nulla auctor, vestibulum magna sed, convallis ex.
              </p>
              <button>View More</button>
            </div>
          ))}
        </div>
        <button>View More Cakes</button>
      </div>

      <div className="categories">
        <h2>Categories</h2>
        <div className="categories-container">
          {["Category 1", "Category 2", "Category 3"].map((category, index) => (
            <div className="category-item" key={index}>
              <img
                src={`https://example.com/cake-category${index + 1}.jpg`}
                alt={category}
              />
              <h3>{category}</h3>
              <button>View Cakes in {category}</button>
            </div>
          ))}
        </div>
      </div>

      <div className="feed-back">
        <h2>FeedBack</h2>
        <div className="feedback-container">
          {[
            {
              text: "I ordered a cake for my birthday and it arrived on time and was absolutely delicious! The staff was friendly and helpful.",
              image: "https://example.com/testimonial-image1.jpg",
              customer: "Customer 1",
            },
            {
              text: "I was looking for a unique and elegant cake for my wedding and Cake Store hit the spot! The cake was beautiful and the service was top-notch.",
              image: "https://example.com/testimonial-image2.jpg",
              customer: "Customer 2",
            },
            {
              text: "I ordered a cake for my daughter's birthday and it was a huge hit! The flavors were amazing and the presentation was beautiful.",
              image: "https://example.com/testimonial-image3.jpg",
              customer: "Customer 3",
            },
          ].map((testimonial, index) => (
            <div className="testimonial-item" key={index}>
              <p>"{testimonial.text}"</p>
              <img src={testimonial.image} alt={`Testimonial ${index + 1}`} />
              <h3>{testimonial.customer}</h3>
            </div>
          ))}
        </div>
        <button>Read More Testimonials</button>
      </div>

      <div className="call-to-action">
        <h2>Get Your Cake Fix Today!</h2>
        <p>Order now and indulge in the sweet life</p>
        <button>Order Now</button>
      </div>

      <div className="footer">
        <p>&copy; 2022 Cake Store. All rights reserved.</p>
        <div className="social-media">
          <a href="https://www.facebook.com/cake-store">
            <img src="https://example.com/facebook-icon.png" alt="Facebook" />
          </a>
          <a href="https://www.instagram.com/cake-store">
            <img src="https://example.com/instagram-icon.png" alt="Instagram" />
          </a>
          <a href="https://www.twitter.com/cake-store">
            <img src="https://example.com/twitter-icon.png" alt="Twitter" />
          </a>
        </div>
        <div className="contact-info">
          <p>Address: 123 Main St, City, State</p>
          <p>Phone: 123-456-7890</p>
          <p>Email: info@cake-store.com</p>
        </div>
      </div>
    </div>
  );
}

export default HomePage;

import React from "react";
import backgroundImage from "./DALL·E 2024-11-26 00.07.50 - A beautifully designed cake set in a professional photoshoot setting, showcasing intricate floral patterns and vibrant colors. The cake is placed on a.webp";
import "./LoginPage.css";

function LoginPage() {
  return (
    <div className="login-page">
      <img
        src={backgroundImage}
        alt="Background Image"
        className="background-image"
      />
      <div className="login-container">
        <div className="login-form">
          <div className="form-container">
            <h1>CakeStore</h1>
            <h2>Login</h2>
            <form name="login-form">
              <div className="form-group">
                <label htmlFor="username">Username:</label>
                <input type="text" id="username" className="medium-input" />
              </div>
              <div className="form-group">
                <label htmlFor="password">Password:</label>
                <input type="password" id="password" className="medium-input" />
              </div>
              <button type="submit">Login</button>
              <p>
                Don't have an account? <a href="#">Sign up</a>
              </p>

              <div className="login-buttons">
                <button className="google-login-btn">
                  <img
                    src="https://developers.google.com/identity/images/g-logo.png"
                    alt="Google Logo"
                  />
                  Google
                </button>
                <button className="facebook-login-btn">
                  <img
                    src="https://www.facebook.com/images/fb_icon_325x325.png"
                    alt="Facebook Logo"
                  />
                  Facebook
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  );
}

export default LoginPage;

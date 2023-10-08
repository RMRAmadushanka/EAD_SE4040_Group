import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import jwtDecode from "jwt-decode";
import Sidebar from "../common/Sidebar";
import TravelAccountManagement from "./TravelAccountManagement";
import { Container, Row } from "react-bootstrap";
export const Backoffice = () => {
  const history = useNavigate();
  const [userData, setUserData] = useState(null);

  // Check if the JWT token is expired
  const token = localStorage.getItem("Token");

  const isTokenExpiredCheck = (token) => {
    try {
      const decodedToken = jwtDecode(token);
      const currentTime = Date.now() / 1000; // Convert to seconds
      return decodedToken.exp < currentTime;
    } catch (error) {
      return true; // Token decoding failed, consider it expired
    }
  };
  const isTokenExpired = token ? isTokenExpiredCheck(token) : false;

  useEffect(() => {
    // Remove the previous token from localStorage when it's expired
    if (isTokenExpired === true) {
      localStorage.removeItem("Token");
      localStorage.clear();
      history("/");
    }
  }, [isTokenExpired]);

  useEffect(() => {
    // Fetch user data from your API or local storage
    const user = JSON.parse(localStorage.getItem("Token"));
    if (user) {
      setUserData(user);
      console.log(userData);
    } else {
      // User is not authenticated, redirect to login
      history("/");
    }
  }, [history]);
  return (
    <div>
      <Container fluid>
        <Row>
          <Sidebar />
          <main className="col-md-20 ml-sm-auto col-lg-10 px-4">
            {/* Use React Router to switch between components based on the selected function */}
            <TravelAccountManagement />
            {/* Add routes to other components here */}
          </main>
        </Row>
      </Container>
      <button onClick={() => history("/")}>Logout</button>
    </div>
  );
};

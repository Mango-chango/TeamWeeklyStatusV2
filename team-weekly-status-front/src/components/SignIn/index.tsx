/// <reference types="../../types.d.ts" />

import React, { useState, useEffect } from "react";
import { Button, Form, Alert, Row, Col } from "react-bootstrap";
import { userStore } from "../../store";
import { useNavigate } from "react-router-dom";
import { makeApiRequest } from "../../services/apiHelper";
import { UserValidationResult } from "../../types/WeeklyStatus.types";
import { GoogleLogin } from "@react-oauth/google";
import "./styles.css";

const SignIn: React.FC = () => {
  const navigate = useNavigate();
  const [emailPrefix, setEmailPrefix] = useState<string>("");
  const [emailDomain] = useState<string>("@mangochango.com");
  const [error, setError] = useState<string | null>(null);

  const loadGoogleScript = () => {
    const script = document.createElement('script');
    script.src = 'https://accounts.google.com/gsi/client'; // URL to the Google API script
    script.onload = () => {
      // Initialize the Google authentication library here
      window.google.accounts.id.initialize({
        client_id: '91039693581-hprbpbenb5fjgm5ccq73d72cpu1o4ptf.apps.googleusercontent.com',
        callback: handleGoogleLogin
      });
      window.google.accounts.id.renderButton(
        document.getElementById('buttonDiv'),
        { theme: 'outline', size: 'large' }  // customization attributes
      );
    };
    document.body.appendChild(script);
  };
  
  useEffect(() => {
    loadGoogleScript();
  }, []);
  

  interface GoogleLoginResponse {
    success: boolean;
    role: "TeamLead" | "CurrentWeekReporter" | "Normal" | null;
    teamName: string | "";
    memberId: number | 0;
    memberName: string | "";
  }

  const handleGoogleLogin = async (response: any) => {
    // You can send the ID token directly to your server and validate it
    const idToken = response.credential;
    try {
      const userResponse: GoogleLoginResponse = await makeApiRequest(
        "/auth/google", // This should be the endpoint in your backend that handles Google login
        "POST",
        { idToken }
      );

      // Assuming your backend returns a similar user object as your existing login


      if (userResponse && userResponse.success) {
        // Set user state and navigate
        userStore
          .getState()
          .setRole(
            userResponse.role as
              | "TeamLead"
              | "CurrentWeekReporter"
              | "Normal"
              | null
          );
        userStore.getState().setTeamName(userResponse.teamName as string | "");
        userStore.getState().setMemberId(userResponse.memberId as number | 0);
        userStore
          .getState()
          .setMemberName(userResponse.memberName as string | "");
        userStore.getState().setIsAuthenticated(true);
        navigate("/weekly-status");
      } else {
        setError("Could not authenticate with Google. Please try again.");
      }
    } catch (error) {
      console.error("Google login error:", error);
      setError("An unexpected error occurred. Please try again.");
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const composedEmail = `${emailPrefix}${emailDomain}`;

    const authRequest = {
      email: composedEmail,
    };

    const response: UserValidationResult = await makeApiRequest(
      "/User/validate",
      "POST",
      authRequest
    );

    if (response && response.success) {
      userStore
        .getState()
        .setRole(
          response.role as "TeamLead" | "CurrentWeekReporter" | "Normal" | null
        );
      userStore.getState().setTeamName(response.teamName as string | "");
      userStore.getState().setMemberId(response.memberId as number | 0);
      userStore.getState().setMemberName(response.memberName as string | "");
      userStore.getState().setIsAuthenticated(true);
      navigate("/weekly-status");
    } else {
      setError("Invalid email address. Please check and try again.");
    }
  };

  return (
    <div className="d-flex flex-column align-items-center mt-5">
      <Form onSubmit={handleSubmit} className="form__container">
        <Form.Group controlId="formEmailPrefix">
          <Form.Label>User</Form.Label>
          <Row>
            <Col>
              <Form.Control
                type="text"
                placeholder="Email prefix"
                value={emailPrefix}
                onChange={(e) => setEmailPrefix(e.target.value)}
              />
            </Col>
            <Col sm={6}>
              <Form.Control type="text" value={emailDomain} readOnly />
            </Col>
          </Row>
        </Form.Group>
        <Form.Group as={Row} className="mb-3 form__buttons" controlId="Boton">
          <Col sm={10}>
            <Button variant="primary" type="submit" className="w-100 mt-3">
              Sign In
            </Button>
            <GoogleLogin
              onSuccess={handleGoogleLogin}
              onError={() =>
                setError("Google Sign-In was unsuccessful. Try again later.")
              }
            />
          </Col>
        </Form.Group>
      </Form>

      {error && (
        <Alert variant="danger" className="mt-3 w-300">
          {error}
        </Alert>
      )}
    </div>
  );
};

export default SignIn;

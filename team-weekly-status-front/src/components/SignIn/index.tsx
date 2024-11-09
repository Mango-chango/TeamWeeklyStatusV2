import React, { useEffect, useState } from "react";
import { Alert, Button, Form, InputGroup } from "react-bootstrap";
import { userStore } from "../../store";
import { useNavigate } from "react-router-dom";
import { makeApiRequest } from "../../services/apiHelper";
import {
  GoogleLoginResponse,
  MemberTeams,
  JungleLoginResponse,
  SupportContact,
  UserProvisioningResponse,
  AuthResponse,
} from "../../types/WeeklyStatus.types";
import { GoogleLogin } from "@react-oauth/google";
import "./styles.css";

const SignIn: React.FC = () => {
  const navigate = useNavigate();
  const {
    setMemberId,
    setMemberName,
    setIsAdmin,
    setIsAuthenticated,
    setMemberActiveTeams,
    setTeamId,
    setTeamName,
    setIsTeamLead,
    setIsCurrentWeekReporter,
    featureFlags,
  } = userStore();

  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [error, setError] = useState<string | null>(null);
  const [infoMessage, setInfoMessage] = useState<string | null>(null);
  const [contactsNotified, setContactsNotified] = useState<SupportContact[]>([]);
  const [showPassword, setShowPassword] = useState(false);
  const [rememberMe, setRememberMe] = useState(false);

  useEffect(() => {
    const storedEmail = localStorage.getItem("email");
    if (storedEmail) {
      setEmail(storedEmail);
      setRememberMe(true);
    }
  }, []);

  const navigateToAppropriatePage = async (memberId: number) => {
    const teamsResponse: MemberTeams = await makeApiRequest(
      `/TeamMember/GetMemberActiveTeams`,
      "POST",
      { id: memberId }
    );

    setMemberActiveTeams(teamsResponse);

    if (teamsResponse.length > 1) {
      navigate("/team-selection");
    } else if (teamsResponse.length === 1) {
      setTeamId(teamsResponse[0].teamId);
      setTeamName(teamsResponse[0].teamName);
      if (teamsResponse[0].isTeamLead) setIsTeamLead(true);
      if (teamsResponse[0].isCurrentWeekReporter) setIsCurrentWeekReporter(true);

      navigate("/weekly-status");
    } else {
      setIsAuthenticated(false);
      setError("You are not associated with any teams.");
      navigate("/");
    }
  };

  const handleGoogleLogin = async (response: any) => {
    const idToken = response.credential;
    try {
      const userResponse: AuthResponse = await makeApiRequest(
        "/Authentication/GoogleLogin",
        "POST",
        { idToken }
      );

      if ("memberId" in userResponse) {
        // User exists in the application database and is authenticated
        setMemberId(userResponse.memberId);
        setMemberName(userResponse.memberName);
        setIsAdmin(userResponse.isAdmin);
        setIsAuthenticated(true);

        await navigateToAppropriatePage(userResponse.memberId);
      } else if ("message" in userResponse) {
        // User has been added but requires configuration
        const provisioningResponse = userResponse as UserProvisioningResponse;
        setInfoMessage(provisioningResponse.message);
        setContactsNotified(provisioningResponse.contactsNotified);
      } else {
        setError("Could not authenticate with Google. Please try again.");
      }
    } catch (error) {
      console.error("Google login error:", error);
      setError("An unexpected error occurred. Please try again.");
    }
  };

  const handleJungleLogin = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      const userResponse: AuthResponse = await makeApiRequest(
        "/Authentication/JungleLogin",
        "POST",
        { email, password }
      );
      if ("memberId" in userResponse) {
        // User exists in the application database and is authenticated
        setMemberId(userResponse.memberId);
        setMemberName(userResponse.memberName);
        setIsAdmin(userResponse.isAdmin);
        setIsAuthenticated(true);

        if (rememberMe) {
          localStorage.setItem("email", email);
        }

        await navigateToAppropriatePage(userResponse.memberId);
      } else if ("message" in userResponse) {
        // User has been added but requires configuration
        const provisioningResponse = userResponse as UserProvisioningResponse;
        setInfoMessage(provisioningResponse.message);
        setContactsNotified(provisioningResponse.contactsNotified);
      } else {
        setError("Could not authenticate with The Jungle. Please try again.");
      }
    } catch (error: any) {
      console.error("Jungle login error:", error);

      if (error.response && error.response.status === 401) {
        setError("Invalid email or password. Please try again.");
      } else {
        setError("An unexpected error occurred. Please try again.");
      }
    }
  };

  const handleShowPassword = () => setShowPassword(!showPassword);

  return (
    <div className="container-main">
      <h2>Welcome to the Team Weekly Status App!</h2>
      {featureFlags.useJungleAuthentication ? (
        <Form onSubmit={handleJungleLogin}>
          <h4>Sign in with your Jungle credentials</h4>
          <Form.Group controlId="email" className="mt-2 pt-3">
            <Form.Control
              type="email"
              placeholder="Enter email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </Form.Group>

          <Form.Group controlId="password" className="mt-2 pt-3">
            <InputGroup>
              <Form.Control
                type={showPassword ? "text" : "password"}
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
                aria-describedby="passwordToggle"
              />
              <Button
                variant="outline-secondary"
                onClick={handleShowPassword}
                aria-label={showPassword ? "Hide password" : "Show password"}
                id="passwordToggle"
              >
                {showPassword ? (
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="16"
                    height="16"
                    fill="currentColor"
                    className="bi bi-eye-slash-fill"
                    viewBox="0 0 16 16"
                  >
                    <path d="M10.477 11.89l-1.823-1.823a3 3 0 1 1 4.243-4.243l1.823 1.823a9 9 0 0 0-4.243 4.243z" />
                    <path d="M13.359 14.36L1.639 2.64l.707-.707 11.72 11.72-.707.707z" />
                  </svg>
                ) : (
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    width="16"
                    height="16"
                    fill="currentColor"
                    className="bi bi-eye-fill"
                    viewBox="0 0 16 16"
                  >
                    <path d="M8 3C3 3 0 8 0 8s3 5 8 5 8-5 8-5-3-5-8-5zm0 8a3 3 0 1 1 0-6 3 3 0 0 1 0 6z" />
                  </svg>
                )}
              </Button>
            </InputGroup>
          </Form.Group>
          <Form.Group controlId="formBasicCheckbox" className="pt-3">
            <Form.Check
              type="checkbox"
              label="Remember me"
              checked={rememberMe}
              onChange={(e) => setRememberMe(e.target.checked)}
            />
          </Form.Group>

          <Button variant="primary" type="submit" className="mt-3 w-100 pt-3">
            Login
          </Button>
        </Form>
      ) : (
        <GoogleLogin
          data-testid="google-login"
          onSuccess={handleGoogleLogin}
          onError={() =>
            setError("Google Sign-In was unsuccessful. Try again later.")
          }
        />
      )}
      {error && (
        <Alert variant="danger" className="mt-3 w-300">
          {error}
        </Alert>
      )}
      {infoMessage && (
        <Alert variant="info" className="mt-3 w-300">
          {infoMessage}
          {contactsNotified.length > 0 && (
            <div>
              <ul>
                {contactsNotified.map((contact, index) => (
                  <li key={index}>
                    {contact.name} ({contact.email})
                  </li>
                ))}
              </ul>
            </div>
          )}
        </Alert>
      )}
    </div>
  );
};

export default SignIn;

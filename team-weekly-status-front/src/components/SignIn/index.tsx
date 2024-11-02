import React, { useEffect, useState } from "react";
import { Alert, Button, Form, InputGroup } from "react-bootstrap";
import { userStore } from "../../store";
import { useNavigate } from "react-router-dom";
import { makeApiRequest } from "../../services/apiHelper";
import {
  GoogleLoginResponse,
  MemberTeams,
  JungleLoginResponse,
} from "../../types/WeeklyStatus.types";
import { GoogleLogin } from "@react-oauth/google";
//import { EyeFill, EyeSlashFill } from 'react-bootstrap-icons';
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
    featureFlags, // Feature flag from userStore
  } = userStore();
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [error, setError] = useState<string | null>(null);
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
      { memberId }
    );

    setMemberActiveTeams(teamsResponse as MemberTeams);

    if (teamsResponse.length > 1) {
      // Navigate to the team selection component if multiple teams are associated
      navigate("/team-selection");
    } else if (teamsResponse.length === 1) {
      // If only one team, set the teamName and navigate to the weekly status page
      setTeamId(teamsResponse[0].teamId as number | 0);
      setTeamName(teamsResponse[0].teamName as string | "");
      teamsResponse[0].isTeamLead && setIsTeamLead(true);
      teamsResponse[0].isCurrentWeekReporter && setIsCurrentWeekReporter(true);

      navigate("/weekly-status");
    } else {
      // If no teams are associated, navigate to the home page
      setIsAuthenticated(false);
      setError("You are not associated with any teams.");
      navigate("/");
    }
  };

  const handleGoogleLogin = async (response: any) => {
    const idToken = response.credential;
    try {
      const userResponse: GoogleLoginResponse = await makeApiRequest(
        "/Authentication/GoogleLogin",
        "POST",
        { idToken }
      );

      if (userResponse && userResponse.success) {
        setMemberId(userResponse.memberId as number | 0);
        setMemberName(userResponse.memberName as string | "");
        setIsAdmin(userResponse.isAdmin as boolean);
        setIsAuthenticated(true);

        // Navigate to the appropriate page based on team association
        await navigateToAppropriatePage(userResponse.memberId);
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
      const jungleLoginResponse: JungleLoginResponse = await makeApiRequest(
        "/Authentication/Login",
        "POST",
        { email, password }
      );
      if (jungleLoginResponse) {
        setMemberId(jungleLoginResponse.memberId as number | 0);
        setMemberName(jungleLoginResponse.memberName as string | "");
        setIsAdmin(jungleLoginResponse.isAdmin as boolean);
        setIsAuthenticated(true);

        if (rememberMe) {
          localStorage.setItem("email", email);
        }

        await navigateToAppropriatePage(jungleLoginResponse.memberId);
      } else {
        setError("Could not authenticate with The Jungle. Please try again.");
      }
    } catch (error) {
      console.error("Jungle login error:", error);
      setError("An unexpected error occurred. Please try again.");
    }
  };
  

  const handleShowPassword = () => setShowPassword(!showPassword);

  return (
    <div className="container-main">
      <h2>Welcome to the Team Weekly Status App!</h2>
      {featureFlags.useJungleAuthentication ? (
        // Render the email/password form for Jungle login
        <Form onSubmit={handleJungleLogin}>
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
                    <path d="m10.79 12.912-1.614-1.615a3.5 3.5 0 0 1-4.474-4.474l-2.06-2.06C.938 6.278 0 8 0 8s3 5.5 8 5.5a7 7 0 0 0 2.79-.588M5.21 3.088A7 7 0 0 1 8 2.5c5 0 8 5.5 8 5.5s-.939 1.721-2.641 3.238l-2.062-2.062a3.5 3.5 0 0 0-4.474-4.474z" />
                    <path d="M5.525 7.646a2.5 2.5 0 0 0 2.829 2.829zm4.95.708-2.829-2.83a2.5 2.5 0 0 1 2.829 2.829zm3.171 6-12-12 .708-.708 12 12z" />
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
                    <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0" />
                    <path d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8m8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7" />
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
        // Render the Google Login button
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
    </div>
  );
};

export default SignIn;

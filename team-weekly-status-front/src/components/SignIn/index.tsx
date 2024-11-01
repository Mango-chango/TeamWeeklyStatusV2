import React, { useState } from "react";
import { Alert, Button, Form } from "react-bootstrap";
import { userStore } from "../../store";
import { useNavigate } from "react-router-dom";
import { makeApiRequest } from "../../services/apiHelper";
import {
  GoogleLoginResponse,
  MemberTeams,
  JungleLoginResponse,
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
    featureFlags, // Feature flag from userStore
  } = userStore();
  const [email, setEmail] = useState<string>(""); // For new auth
  const [password, setPassword] = useState<string>(""); // For new auth
  const [error, setError] = useState<string | null>(null);

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
        "/GoogleAuth",
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

        await navigateToAppropriatePage(jungleLoginResponse.memberId);
      } else {
        setError("Could not authenticate with The Jungle. Please try again.");
      }
    } catch (error) {
      console.error("Jungle login error:", error);
      setError("An unexpected error occurred. Please try again.");
    }
  };

  return (
    <div className="container-main">
      <h2>Welcome to the Team Weekly Status App!</h2>
      <h3>Sign in</h3>
      {featureFlags.useJungleAuthentication ? (
        // Render the email/password form for Jungle login
        <Form onSubmit={handleJungleLogin}>
          <Form.Group controlId="email">
            <Form.Label>Email address</Form.Label>
            <Form.Control
              type="email"
              placeholder="Enter email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </Form.Group>

          <Form.Group controlId="password" className="mt-2">
            <Form.Label>Password</Form.Label>
            <Form.Control
              type="password"
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </Form.Group>

          <Button variant="primary" type="submit" className="mt-3">
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

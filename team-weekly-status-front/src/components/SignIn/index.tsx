import React, { useState, useEffect } from "react";
import { Alert } from "react-bootstrap";
import { userStore } from "../../store";
import { useNavigate } from "react-router-dom";
import { makeApiRequest } from "../../services/apiHelper";
import {
  GoogleLoginResponse,
  MemberTeams,
  Team,
} from "../../types/WeeklyStatus.types";
import { GoogleLogin } from "@react-oauth/google";
import "./styles.css";

const SignIn: React.FC = () => {
  const navigate = useNavigate();
  const [emailPrefix, setEmailPrefix] = useState<string>("");
  const [emailDomain] = useState<string>("@mangochango.com");
  const [error, setError] = useState<string | null>(null);
  //const [userTeams, setUserTeams] = useState<MemberTeams>([]);

  const loadGoogleScript = () => {
    const script = document.createElement("script");
    script.src = "https://accounts.google.com/gsi/client"; // URL to the Google API script
    script.onload = () => {
      // Initialize the Google authentication library here
      window.google.accounts.id.initialize({
        client_id:
          "91039693581-hprbpbenb5fjgm5ccq73d72cpu1o4ptf.apps.googleusercontent.com",
        callback: handleGoogleLogin,
      });
      window.google.accounts.id.renderButton(
        document.getElementById("buttonDiv"),
        { theme: "outline", size: "large" } // customization attributes
      );
    };
    document.body.appendChild(script);
  };

  useEffect(() => {
    loadGoogleScript();
  }, []);

  const handleGoogleLogin = async (response: any) => {
    const idToken = response.credential;
    // console.log("idToken=", idToken);
    try {
      const userResponse: GoogleLoginResponse = await makeApiRequest(
        "/GoogleAuth",
        "POST",
        { idToken }
      );

      if (userResponse && userResponse.success) {
        // console.log("userResponse=", userResponse);
        userStore.getState().setMemberId(userResponse.memberId as number | 0);
        userStore
          .getState()
          .setMemberName(userResponse.memberName as string | "");
        userStore.getState().setIsAdmin(userResponse.isAdmin as boolean);
        userStore.getState().setIsAuthenticated(true);

        const teamsResponse: MemberTeams = await makeApiRequest(
          `/TeamMember/GetMemberActiveTeams`,
          "POST",
          { memberId: userResponse.memberId }
        );
        // console.log("teamsResponse=", teamsResponse);
        userStore.getState().setMemberActiveTeams(teamsResponse as MemberTeams);

        if (teamsResponse.length > 1) {
          // Navigate to the team selection component if multiple teams are associated
          navigate("/team-selection");
        } else  if (teamsResponse.length === 1) {
          // If only one team, set the teamName and navigate to the weekly status page
          userStore.getState().setTeamId(teamsResponse[0].teamId as number | 0);
          userStore
            .getState()
            .setTeamName(teamsResponse[0].teamName as string | "");
          teamsResponse[0].isTeamLead && userStore.getState().setIsTeamLead(true);
          teamsResponse[0].isCurrentWeekReporter &&
            userStore.getState().setIsCurrentWeekReporter(true);

          navigate("/weekly-status");
        } else {
          // If no teams are associated, navigate to the home page
          userStore.getState().setIsAuthenticated(false);
          setError("You are not associated with any teams.");
          navigate("/");
        }
      } else {
        setError("Could not authenticate with Google. Please try again.");
      }
    } catch (error) {
      console.error("Google login error:", error);
      setError("An unexpected error occurred. Please try again.");
    }
  };

  return (
    <div className="container-main">
      <h2>Welcome to the Team Weekly Status App!</h2>
      <h3>Sign in using your MangoChango account</h3>
      <GoogleLogin
        data-testid="google-login"
        onSuccess={handleGoogleLogin}
        onError={() =>
          setError("Google Sign-In was unsuccessful. Try again later.")
        }
      />
      {error && (
        <Alert variant="danger" className="mt-3 w-300">
          {error}
        </Alert>
      )}
    </div>
  );
};

export default SignIn;

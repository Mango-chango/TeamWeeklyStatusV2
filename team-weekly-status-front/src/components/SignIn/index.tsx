import React, { useState, useEffect } from "react";
import { Alert } from "react-bootstrap";
import { userStore } from "../../store";
import { useNavigate } from "react-router-dom";
import { makeApiRequest } from "../../services/apiHelper";
import { GoogleLoginResponse, MemberTeams, Team } from '../../types/WeeklyStatus.types';
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
    console.log("idToken=", idToken);
    try {
      const userResponse: GoogleLoginResponse = await makeApiRequest(
        "/GoogleAuth",
        "POST",
        { idToken }
      );

      if (userResponse && userResponse.success) {
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

        const teamsResponse: MemberTeams = await makeApiRequest(
          `/TeamMember/GetMemberActiveTeams`,
          "POST",
          { memberId: userResponse.memberId }
        )
        userStore.getState().setMemberActiveTeams(teamsResponse as MemberTeams);

        if (teamsResponse.length > 1) {
          // Navigate to the team selection component if multiple teams are associated
          navigate("/team-selection");
        } else {
          // If only one team, set the teamName and navigate to the weekly status page
          userStore.getState().setTeamName(teamsResponse[0].teamName as string | "");
          navigate("/weekly-status");
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
    <div className="d-flex flex-column align-items-center mt-5">
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

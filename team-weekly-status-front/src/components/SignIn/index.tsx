import React, { useState } from "react";
import { Button, Form, Alert, Row, Col } from "react-bootstrap";
import { userStore } from "../../store";
import { useNavigate } from "react-router-dom";
import { makeApiRequest } from "../../services/apiHelper";
import { UserValidationResult } from "../../types/WeeklyStatus.types";
import './styles.css';

const SignIn: React.FC = () => {
  const navigate = useNavigate();
  const [emailPrefix, setEmailPrefix] = useState<string>("");
  const [emailDomain] = useState<string>("@mangochango.com");
  const [error, setError] = useState<string | null>(null);

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
        <Form.Group
          as={Row}
          className="mb-3 form__buttons"
          controlId="Boton"
        >
          <Col sm={10}>
            <Button variant="primary" type="submit" className="w-100 mt-3">
              Sign In
            </Button>
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

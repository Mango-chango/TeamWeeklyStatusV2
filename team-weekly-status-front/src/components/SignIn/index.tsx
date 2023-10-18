import React, { useState } from "react";
import { Button, Form, Alert, Row, Col } from "react-bootstrap";
import { userStore } from '../../store';
import { useNavigate } from "react-router-dom";
import { makeApiRequest } from "../../services/apiHelper";
import { UserValidationResult } from "../../types/WeeklyStatus.types";

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
        userStore.getState().setRole(response.role as "TeamLead" | "CurrentWeekReporter" | "Normal" | null);
        navigate("/weekly-status");
    } else {
        setError("Invalid email address. Please check and try again.");
    }
  };

  return (
    <div className="d-flex flex-column align-items-center mt-5">
      <Form onSubmit={handleSubmit} style={{ width: "600px" }}>
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
          className="mb-3"
          controlId="Boton"
          style={{
            paddingTop: "15px",
            marginLeft: "auto",
            marginRight: "auto",
            paddingLeft: "150px",
          }}
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

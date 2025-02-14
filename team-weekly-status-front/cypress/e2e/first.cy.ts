/// <reference types="cypress" />

describe("User Authentication Flow", () => {
  it("should navigate to the sign-in page, log in, select a team, and arrive to the weekly report page", () => {
    cy.visit("https://localhost:5173/", {
      failOnStatusCode: false, // to bypass certificate error
    });

    // Login
    cy.get('[data-testid="email-input"]').type("lgarcia@mangochango.com");
    cy.get('[data-testid="password-input"]').type("userMangoChango19$");
    cy.get('[data-testid="login-button"]').click();

    // Verify redirection to team selection page
    cy.url().should("include", "/team-selection"); // Assuming the team selection page is at /team-selection

    // Select the 'Siepe' team
    cy.get('[data-testid="team-list"]').contains("Siepe").click();
    cy.get("button").contains("Continue").click();

    // Verify redirection to weekly status page
    cy.url().should("include", "/weekly-status"); // Assuming the weekly status page is at /weekly-status
    cy.contains("Team Siepe").should("be.visible"); // Ensure header title is present
  });

  /* ==== Test Created with Cypress Studio ==== */
  it('studio1', function() {
    /* ==== Generated with Cypress Studio ==== */
    cy.visit('https://localhost:5173/');
    /* ==== End Cypress Studio ==== */
    /* ==== Generated with Cypress Studio ==== */
    cy.get('[data-testid="email-input"]').click();
    cy.get('[data-testid="email-input"]').should('be.visible');
    cy.get('[data-testid="login-button"]').should('be.visible');
    cy.get('[data-testid="login-button"]').click();
    cy.get('[data-testid="email-input"]').clear('lgarcia@mangochango.com');
    cy.get('[data-testid="email-input"]').type('lgarcia@mangochango.com');
    cy.get('[data-testid="login-button"]').click();
    cy.get('[data-testid="password-input"]').clear();
    cy.get('[data-testid="password-input"]').type('holahola');
    cy.get('path').click();
    cy.get('[data-testid="login-button"]').click();
    /* ==== End Cypress Studio ==== */
  });
});

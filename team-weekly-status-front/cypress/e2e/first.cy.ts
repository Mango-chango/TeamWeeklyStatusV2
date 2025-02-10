
/// <reference types="cypress" />

describe('User Authentication Flow', () => {
  it('should navigate to the sign-in page, log in, select a team, and arrive to the weekly report page', () => {
    cy.visit('/'); // Assuming the sign-in page is at the /signin route

    cy.get('button').contains('Login').should('be.visible');
    cy.get('[data-testid="email-input"]').type('lgarcia@mangochango.com'); // Replace 'your_username'
    cy.get('[data-testid="password-input"]').type('userMangoChango19$'); // Replace 'your_password'
    cy.get('button').contains('Login').click();
    
    // Verify redirection to team selection page
    cy.url().should('include', '/team-selection'); // Assuming the team selection page is at /team-selection
  
    // Select the first team
    // cy.get('[data-testid="team-list"]')
    // .select('Siepe')
    // .should('have.value', 'Siepe');
     //cy.get('button').contains('Siepe').click(); // Replace 'Sirpe' with the actual team name
     cy.get('[data-testid="team-list"]').contains('Siepe').click();
    cy.get('button').contains('Continue').click();
    
    // Verify redirection to weekly status page
    cy.url().should('include', '/weekly-status'); // Assuming the weekly status page is at /weekly-status
     cy.contains('Team Siepe').should('be.visible') // Ensure header title is present
  });
});
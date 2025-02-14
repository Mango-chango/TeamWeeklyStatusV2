describe('template spec', () => {
  it('passes', () => {
    cy.visit('https://example.cypress.io')
  })

  /* ==== Test Created with Cypress Studio ==== */
  it('wrongpwd', function() {
    /* ==== Generated with Cypress Studio ==== */
    cy.visit('https://localhost:5173/');
    cy.get('[data-testid="email-input"]').clear('lg');
    cy.get('[data-testid="email-input"]').type('lgarcia@mangochango.com');
    cy.get('[data-testid="password-input"]').clear();
    cy.get('[data-testid="password-input"]').type('holahola');
    cy.get('#formBasicCheckbox').clear();
    cy.get('#formBasicCheckbox').type('on');
    cy.get('[data-testid="login-button"]').click({force: true});
    /* ==== End Cypress Studio ==== */
  });
})
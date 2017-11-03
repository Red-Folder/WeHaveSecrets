// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************
//
//
// -- This is a parent command --
// Cypress.Commands.add("login", (email, password) => { ... })
//
//
// -- This is a child command --
// Cypress.Commands.add("drag", { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add("dismiss", { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This is will overwrite an existing command --
// Cypress.Commands.overwrite("visit", (originalFn, url, options) => { ... })

Cypress.Commands.add("register", (username) => {
    cy.get('.register-button')
        .click()
    cy.get('#Username')
        .type(username)
    cy.get('#Password')
        .type('Test1234!')
    cy.get('#ConfirmPassword')
        .type('Test1234!')
    cy.get('button[type=submit]')
        .click()
})

Cypress.Commands.add("login", (username) => {
    cy.get('.login-button')
        .click()

    // Complete the login page
    cy.get('#Username')
        .type(username)
    cy.get('#Password')
        .type('Test1234!')
    cy.get('button[type=submit]')
        .click()
})

Cypress.Commands.add("logout", () => {
    cy.get('.logout-button')
        .click()
})

Cypress.Commands.add("addSecret", (secretKey, secretValue) => {
    cy.get('.secrets-link')
        .click()
    cy.get('#Key')
        .type(secretKey)
    cy.get('#Value')
        .type(secretValue)
    cy.get('#SaveButton')
        .click()
})


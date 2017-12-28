describe('As a SecretsRUs customer', function(){
    it('I should be able to register, and login', function(){
        cy.visit(Cypress.env('WEHAVESECRETS_URL'))

        let username = 'user' + (new Date()).toISOString().replace(/[^0-9]/g, "")

        // Register
        cy.register(username)

        // Logout
        cy.logout()

        // Log back in
        cy.login(username)

        // Logout
        cy.logout()
    })

    it('I should be able to register, and add a secret, collect the link and then access anonymously', function(){
        cy.visit(Cypress.env('WEHAVESECRETS_URL'))

        let username = 'user' + (new Date()).toISOString().replace(/[^0-9]/g, "")

        // Register
        cy.register(username)

        // Add a Secret
        let secretKey = 'Key1';
        let secretValue = 'Value1';
        cy.addSecret(secretKey, secretValue)

        // Logout
        cy.logout()

        // Log back in
        cy.login(username)
        
        // Validate that the secret is there
        cy.get('.secrets-link')
            .click()
        let secretList = cy.get('.secrets-list')
        secretList.contains(secretValue)

        // Navigate to the Public Share
        let shareLink = "";
        cy.get('.secrets-list').within(function() {
            cy.get(".panel:contains('" + secretKey + "')").within(function() {
                cy.get("a").then(($link) => {
                    cy.visit($link.prop('href'))
                        //.then(() => shareLink = cy.url())
                })
            })
        })

        // Store the current public share
        cy.url().then((link) => shareLink = link)

        // Logout & open the share as a public user
        cy.logout()
            .then(() => {
                cy.visit(shareLink)
            })
    })

})
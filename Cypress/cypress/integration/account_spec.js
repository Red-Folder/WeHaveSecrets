describe('As a SecretsRUs customer', function(){
    it('I should be able to register, and login', function(){
        cy.visit('http://localhost:44353/')

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

    it('I should be able to register, and add a secret', function(){
        cy.visit('http://localhost:44353/')

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
        cy.get('.secrets-list')
            .contains(secretValue)
    })

})
describe('As a SecretsRUs customer', function(){
    it('I should be able to register, and login', function(){
        cy.visit('https://localhost:44353/')

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
})
#FROM cypress/browsers:chrome62
FROM cypress/base

USER root
WORKDIR /app

# Install NPM modules
COPY package.json .
RUN npm install

COPY cypress.json .
COPY cypress cypress

RUN $(npm bin)/cypress verify
RUN $(npm bin)/cypress run

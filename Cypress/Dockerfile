#FROM cypress/browsers:chrome62
FROM cypress/base

USER root
WORKDIR /app

# Install NPM modules
COPY package.json .
RUN npm install

COPY cypress.json .
COPY cypress cypress

RUN node_modules/.bin/cypress verify

CMD ["node_modules/.bin/cypress", "run"]

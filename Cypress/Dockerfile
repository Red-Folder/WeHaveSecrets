FROM cypress/browsers:chrome62

WORKDIR /app
COPY cypress cypress
RUN npm install --save-dev cypress
RUN $(npm bin)/cypress verify
RUN $(npm bin)/cypress run --browser chrome

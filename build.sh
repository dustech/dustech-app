#!/bin/bash

npm run sass:build-for-dotnet && \
cp ./node_modules/bootstrap/dist/js/bootstrap.bundle.min.js ./src/Dustech.App.Web/wwwroot/js/
#!/bin/bash

npm run sass:build && \
cp ./node_modules/bootstrap/dist/js/bootstrap.bundle.min.js ./src/Dustech.App.Web/wwwroot/js/
cp ./node_modules/bootstrap-icons/font/fonts/* ./src/Dustech.App.Web/wwwroot/styles/fonts/
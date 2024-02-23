#!/bin/bash


# Check if directory exists
if [ ! -d "./src/Dustech.App.Web/wwwroot/styles/fonts/" ]; then
  # If not, create
  mkdir -p ./src/Dustech.App.Web/wwwroot/styles/fonts/
fi


npm run sass:build && \
cp ./node_modules/bootstrap/dist/js/bootstrap.bundle.min.js ./src/Dustech.App.Web/wwwroot/js/
cp ./node_modules/bootstrap-icons/font/fonts/* ./src/Dustech.App.Web/wwwroot/styles/fonts/
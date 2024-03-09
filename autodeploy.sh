#!/bin/bash

sudo wg-quick down wg-client1 && \
git pull && \
./docker-build-all && \
./runna && \
sudo wg-quick up wg-client1
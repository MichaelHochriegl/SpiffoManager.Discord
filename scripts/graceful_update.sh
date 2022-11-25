#!/bin/bash
# Save this script as "graceful.sh"
# Make it executable with "chmod +x graceful.sh"
# Then run it when you want to notify players 15 minutes before the
# server restarts with:
#   ./graceful.sh
session=$1
tmux send-keys -t $session "servermsg \"Check auf neue Version...\"" Enter
sleep 5
./$session update


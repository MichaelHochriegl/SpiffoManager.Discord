#!/bin/bash
# Save this script as "get_players.sh"
# Make it executable with "chmod +x get_players.sh"
session=$1
tmux pipe-pane -t $session 'cat >/tmp/capture'
tmux send-keys -t $session "players" Enter
sleep 2
tmux pipe-pane -t $session
output=$(</tmp/capture)
echo $output | grep -oE 'Players connected \(([[:digit:]]+)\)' | grep -oE '[[:digit:]]+'

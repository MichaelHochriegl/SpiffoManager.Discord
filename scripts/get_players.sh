#!/bin/bash
# Save this script as "get_players.sh"
# Make it executable with "chmod +x get_players.sh"
session=$(tmux list-sessions |  cut -d: -f1)
tmux send-keys -t $session "players" Enter
sleep 1
output=$(tmux capture-pane -p -t $session -E -1 | grep -oE 'Players connected \(([[:digit:]]+)\)' | grep -oE '[[:digit:]]+')
echo $output

#!/bin/bash
# Save this script as "graceful.sh"
# Make it executable with "chmod +x graceful.sh"
# Then run it when you want to notify players 15 minutes before the
# server restarts with:
#   ./graceful.sh
session=$1
for x in {$2..2..-1}
do
  tmux send-keys -t $session "servermsg \"Neustart in: $x Minuten\"" Enter
  sleep 60
done

for x in {60..10..-10}
do
  tmux send-keys -t $session "servermsg \"Neustart in: $x Sekunden\"" Enter
  sleep 10
done

tmux send-keys -t $session  "servermsg \"Neustart des Servers jetzt!\"" Enter
sleep 5
~/pzserver stop
sleep 10
git stash
sleep 5
git pull
sleep 20
rm ~/serverfiles/steamapps/workshop/appworkshop_108600.acf
rm -R ~/serverfiles/steamapps/workshop/content
sleep 5
~/pzserver start


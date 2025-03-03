
## TODO
- workflow_state beim betritt der seite gleich erzeugen wie beim publish
- bei statuswechsel sollte workflow_state gepublisht werden (wegen status==1)_
- die Log einträge eines workflows sollten irgendwie persistiert werden
- workflow statusänderungen sollten live sichtbar sein
	- evtl bekommt man die nur per middleware mit?
	- resume button nur anzeigen wenn status==1
- BK wäre gut, bei Assetverzeichnis und als Workflowinstance_id wenn möglich_
  und beim betreten eines Workflow angezeigt werden
- auto-archivieren / löschen alter workflows inkl assets
- bei "resume" sollte geloggt werden wer wann geklickt hat

## History

- added client side library @microsoft/signalr
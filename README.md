
## TODO
- workflow_state beim betritt der seite gleich erzeugen wie beim publish
- bei statuswechsel sollte workflow_state gepublisht werden (wegen status==1)_
- die Log eintr�ge eines workflows sollten irgendwie persistiert werden
- workflow status�nderungen sollten live sichtbar sein
	- evtl bekommt man die nur per middleware mit?
	- resume button nur anzeigen wenn status==1
- BK w�re gut, bei Assetverzeichnis und als Workflowinstance_id wenn m�glich_
  und beim betreten eines Workflow angezeigt werden
- auto-archivieren / l�schen alter workflows inkl assets
- bei "resume" sollte geloggt werden wer wann geklickt hat

## History

- added client side library @microsoft/signalr
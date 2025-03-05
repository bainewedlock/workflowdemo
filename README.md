
## TODO
- Logs
  - beim ersten aufruf sollten die logs im WorkflowState mitkommen
  - danach nicht mehr (weil sie ja eh live geadded werden)
- suspended sollte man auch abbrechen / parken? können
- Log Tail auf der Workflow Page
	- evtl machts auch sinn die logs und die
	  errors
	  und dann beides in einer liste auszugeben.
	- dann scrollt man einfach über die page
- Workflow Page schöner machen (Log + Error Tabellen scrollbar)
  - Status farblich hinterlegen
  - start/end kleiner
- BK wäre gut, bei Assetverzeichnis und als Workflowinstance_id wenn möglich_
  und beim betreten eines Workflow angezeigt werden
- auto-archivieren / löschen alter workflows inkl assets
- bei "resume" sollte geloggt werden wer wann geklickt hat
- maintenance seiten:
  - integrity check
    - zeige asset folder ohne workflow und umgekehrt
	- auf knopfdruck löschen

## History

- added client side library @microsoft/signalr
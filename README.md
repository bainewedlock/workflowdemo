
## TODO
- Logs
  - die ExecutionErrors nicht mehr anzeigen (string date ist eh weird)
  - stattdessen im WalzStepBodyAsync auch die exceptions loggen
- suspended sollte man auch abbrechen / parken? k�nnen
- Log Tail auf der Workflow Page
	- evtl machts auch sinn die logs und die
	  errors
	  und dann beides in einer liste auszugeben.
	- dann scrollt man einfach �ber die page
- Workflow Page sch�ner machen (Log + Error Tabellen scrollbar)
  - Status farblich hinterlegen
  - start/end kleiner
- BK w�re gut, bei Assetverzeichnis und als Workflowinstance_id wenn m�glich_
  und beim betreten eines Workflow angezeigt werden
- auto-archivieren / l�schen alter workflows inkl assets
- bei "resume" sollte geloggt werden wer wann geklickt hat
- maintenance seiten:
  - integrity check
    - zeige asset folder ohne workflow und umgekehrt
	- auf knopfdruck l�schen

## History

- added client side library @microsoft/signalr
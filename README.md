
## TODO
- InstanceTitle als Assetverzeichnis nehmen?
  - �berlegung: (wo/wie) m�chte man vermeiden dass f�r
    ein WorkItem mehrere Workflows gestartet werden?
    (beispiel: DPKDSRETL -> StartWorkflow("label", null, BK))
    dran denken dass der workflow und das
    assetverzeichnis ggf schon archiviert sind
  - sicherstellen, dass beim start eines workflows das assetverzeichnis 
    noch nicht existiert?
- auto-archivieren / l�schen alter workflows inkl assets
- maintenance seiten:
  - integrity check
    - zeige asset folder ohne workflow und umgekehrt
	- auf knopfdruck l�schen

## History

- added client side library @microsoft/signalr
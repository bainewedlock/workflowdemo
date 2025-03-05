
## TODO
- InstanceTitle als Assetverzeichnis nehmen?
  - Überlegung: (wo/wie) möchte man vermeiden dass für
    ein WorkItem mehrere Workflows gestartet werden?
    (beispiel: DPKDSRETL -> StartWorkflow("label", null, BK))
    dran denken dass der workflow und das
    assetverzeichnis ggf schon archiviert sind
  - sicherstellen, dass beim start eines workflows das assetverzeichnis 
    noch nicht existiert?
- auto-archivieren / löschen alter workflows inkl assets
- maintenance seiten:
  - integrity check
    - zeige asset folder ohne workflow und umgekehrt
	- auf knopfdruck löschen

## History

- added client side library @microsoft/signalr
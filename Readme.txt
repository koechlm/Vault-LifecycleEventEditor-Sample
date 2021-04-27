LIFECYCLE EVENT EDITOR


INTRODUCTION:
---------------------------------
The lifecycle event editor allows the setting of custom events to fire when a file, item or change order moves to a different lifecycle state.  The purpose of firing the event is to create a new job on the Job queue.  


FEATURES:
---------------------------------
- The ability to add, remove, or delete custom job events.


REQUIREMENTS:
---------------------------------
- Vault Workgroup or Vault Professional
- The Job Service feature must be running


TO ADD A NEW LIFECYCLE EVENT:
---------------------------------
- Run LifecycleEventEditor.exe
- Log in
- Select the tab for the object type you want to work with.
- Select the lifecycle definition
- Selete the state.  It can either be the start or end state.
- Select the transition.
- Run the "Add Job" command or double click the "Job Types" box to add a new job to the transition.
- Type the name of the job type, which determines which handler will process it.
- Run the "Commit Changes" command so save the data.
- Exit the program.  Now when a file goes through the lifecycle event, a job of your type will get added to the job queueu.


NOTES:
---------------------------------
- Don't set a custom job event if there is no handler for that type.  Otherwise you will get a bunch of Jobs on the queue that will just sit there instead if getting processed.

- These lifecyce events are always post events and they do not get processed immediately.

- You can fire multiple custom jobs in a single transition.

- Keep in mind that every file that does through the transition will fire the job.  For example if a large assembly, with 1000's of files, changes state, then you will get 1000s of custom jobs on the queue.  The same is true for items and change orders.

- This tool is only for custom job types.  Built-in job types, such as DWF creation and property sync, are still controlled through the Vault clients.


SUPPORT:
---------------------------------
No official support is available for this tool.  However you can post questions or comments to the Vault newsgroup http://forums.autodesk.com/t5/Autodesk-Vault/bd-p/101

VERSION HISTORY:
---------------------------------
2022.0.0.0 - updated for 2022 compatibility
2021.26.0.0 - updated for 2021
2020.25.0.1 - Fixed licensing issue
2020.25.0.0 - updated for 2020, including click licensing v2

24.0.0.0 based on SDK 2017: 22.0.56.0 including these changes: updated to SDK 2019 references, included clmloader.dll in project 


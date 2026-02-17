/**************************************************************************/
/* LabWindows/CVI User Interface Resource (UIR) Include File              */
/*                                                                        */
/* WARNING: Do not add to, delete from, or otherwise modify the contents  */
/*          of this include file.                                         */
/**************************************************************************/

#include <userint.h>

#ifdef __cplusplus
    extern "C" {
#endif

     /* Panels and Controls: */

#define  MAINPANEL                        1       /* callback function: MainPanelCallback */
#define  MAINPANEL_FILESCOMBO             2       /* control type: activeX, callback function: (none) */
#define  MAINPANEL_OPENFILEBTN            3       /* control type: activeX, callback function: (none) */
#define  MAINPANEL_SEQUENCESCOMBO         4       /* control type: activeX, callback function: (none) */
#define  MAINPANEL_CLOSEFILEBTN           5       /* control type: activeX, callback function: (none) */
#define  MAINPANEL_ENTRYPOINT1BTN         6       /* control type: activeX, callback function: (none) */
#define  MAINPANEL_ENTRYPOINT2BTN         7       /* control type: activeX, callback function: (none) */
#define  MAINPANEL_RUNSELECTEDBTN         8       /* control type: activeX, callback function: (none) */
#define  MAINPANEL_EXECUTIONSCOMBO        9       /* control type: activeX, callback function: (none) */
#define  MAINPANEL_CLOSEEXECUTIONBTN      10      /* control type: activeX, callback function: (none) */
#define  MAINPANEL_SEQUENCEVIEW           11      /* control type: activeX, callback function: (none) */
#define  MAINPANEL_BREAKRESUMEBTN         12      /* control type: activeX, callback function: (none) */
#define  MAINPANEL_TERMINATERESTARTBTN    13      /* control type: activeX, callback function: (none) */
#define  MAINPANEL_TERMINATEALLBTN        14      /* control type: activeX, callback function: (none) */
#define  MAINPANEL_APPLICATIONMGR         15      /* control type: activeX, callback function: (none) */
#define  MAINPANEL_SEQUENCEFILEVIEWMGR    16      /* control type: activeX, callback function: (none) */
#define  MAINPANEL_EXECUTIONVIEWMGR       17      /* control type: activeX, callback function: (none) */
#define  MAINPANEL_REPORTVIEW             18      /* control type: activeX, callback function: (none) */
#define  MAINPANEL_LOGINLOGOUTBTN         19      /* control type: activeX, callback function: (none) */
#define  MAINPANEL_EXITBTN                20      /* control type: activeX, callback function: (none) */


     /* Control Arrays: */

          /* (no control arrays in the resource file) */


     /* Menu Bars, Menus, and Menu Items: */

          /* (no menu bars in the resource file) */


     /* Callback Prototypes: */

int  CVICALLBACK MainPanelCallback(int panel, int event, void *callbackData, int eventData1, int eventData2);


#ifdef __cplusplus
    }
#endif

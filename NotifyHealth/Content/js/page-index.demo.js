/*   
Template Name: Source Admin - Responsive Admin Dashboard Template build with Twitter Bootstrap 3.3.7 & Bootstrap 4
Version: 1.5.0
Author: Sean Ngu
Website: http://www.seantheme.com/source-admin-v1.5/admin/
*/
    
var purple = '#9b59b6';
var purpleLight = '#BE93D0';
var purpleDark = '#7c4792';
var success = '#17B6A4';
var successDark = '#129283';
var primary = '#2184DA';
var primaryDark = '#1e77c5';
var info = '#38AFD3';
var inverse = '#3C454D';
var warning = '#fcaf41';
var danger = '#F04B46';
var dangerLight = '#F58A87';
var dangerDark = '#c03c38';
var lime = '#65C56F';
var grey = '#aab3ba';
var white = '#fff';
var fontFamily = 'inherit';
var fontWeight = 'normal';
var fontStyle = 'normal';



/* Application Controller
------------------------------------------------ */
var PageDemo = function () {
	"use strict";
	
	return {
		//main function
		init: function () {
		    handleVisitorAnalyticsChart();
		    handleVisitorsVectorMap();
		    handleServerChart();
		    handleWidgetChat();
		    handleWidgetReload();
		    handleWidgetTodolist();
		    handleDashboardGritterNotification();
		}
    };
}();
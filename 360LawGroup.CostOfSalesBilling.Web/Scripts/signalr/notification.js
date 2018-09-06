$(function () {
    var notificaitonHub = $.connection.notificaitonHub;
    notificaitonHub.client.pushMessage = function (data) {
        var obj = JSON.parse(data);
        if (obj.Type === "NotifyInit") {
            addNotification(obj.Data);
        } 
    };
    $.connection.hub.start();
    GetUpdateNotification(0);
    $("#header_notification_bar .icon-bell").on("click", function () {
        $(".notifyjs-corner").html("");
    });
    $(document).on("click", ".notifyjs-corner a.content", function () {
        showNotifyMenu();
    });
});
var notificationInterval = null;
function GetUpdateNotification(offset) {
    var bottom = $(".notification_bottom").parent();
    bottom.hide();
    $("#header_notification_bar > a > span.badge").html("0");
    $("#loadmorenotification").remove();
    if (offset == 0) {
        $("li[id^='notification_']").remove();
    }
    apiCall("common/getnotifications?offset=" + offset, "get", {}, function (data) {
        if (data.total > 0) {
            $(".nonotification").remove();
            $(data.rows).each(function (i, item) {
                var html = $(getHtmlTemplate(item));
                if (item.State < 2) {
                    html.find("a.notification-remove").remove();
                }
                $("li[id='notification_" + item.NotificationId + "']").remove();
                html.appendTo("#notificaiton_list");
            });
            $("#loadmorenotification").remove();
            if ((offset + 1) * 5 < data.total) {
                offset++;
                $('<li id="loadmorenotification" class="loadmore text-center"><a href="javascript:;" class="" onclick="GetUpdateNotification(' + offset + ');showNotifyMenu();">View More</a></li>').appendTo("#notificaiton_list");
            }
            updateNotificationTime();
        } else {
            if (notificationInterval != null)
                clearInterval(notificationInterval);
            $('<li class="nonotification text-center">No notifications</li>').appendTo("#notificaiton_list");
        }
        updateNotificationCount(data.total);
    });
}
function getHtmlTemplate(obj) {
    var itemhtm = "<li id='notification_" + obj.NotificationId + "'>";
    itemhtm += "<div class='notification-item'>";
    itemhtm += "<div class='content " + notificationState(obj.State) + "' href='javascript:;' style='padding:5px 10px 0 10px'>";
    itemhtm += "<p class='item-info' title='" + getNotificationTitle(obj) + "'>";
    itemhtm += "<strong class='item-title'>" + kendohtmlDecode(notificationTitle(obj.State));
    itemhtm += "</strong>";
    itemhtm += "<strong>" + obj.Title + "</strong> - " + getMessageForNotifications(obj.Message) + "";
    itemhtm += "</p >";
    itemhtm += "<span>" + notificationTimeSpan(obj.State, obj.UpdatedOn) + "</span>";
    itemhtm += "<a class='notification-remove' href='javascript:;' onclick='removeNotification(\"" + obj.NotificationId + "\")'><i class='fa fa-times'></i></a>";
    itemhtm += "</div>";
    itemhtm += "</div>";
    itemhtm += "</li>";
    return itemhtm;
}
function showNotifyMenu() {
    setTimeout(function () {
        $("#header_notification_bar").removeClass("open").addClass("open");
    }, 100);
}
function updateNotificationTime() {
    if (notificationInterval != null) { clearInterval(notificationInterval); }
    notificationInterval = setInterval(function () {
        $("#header_notification_bar ul li span[data-datetime]").each(function () {
            var dateValue = $(this).attr("data-datetime");
            var state = $(this).attr("data-state");
            $(this).text(notificationTimeSpan(state, dateValue));
        });
    }, 60000);
}
function time_ago(time) {
    switch (typeof time) {
        case "number": break;
        case "string": time = +new Date(time); break;
        case "object": if (time.constructor === Date) time = time.getTime(); break;
        default: time = +new Date();
    }
    var time_formats = [
        [60, "seconds", 1], // 60
        [120, "1 minute ago", "1 minute from now"], // 60*2
        [3600, "minutes", 60], // 60*60, 60
        [7200, "1 hour ago", "1 hour from now"], // 60*60*2
        [86400, "hours", 3600], // 60*60*24, 60*60
        [172800, "Yesterday", "Tomorrow"], // 60*60*24*2
        [604800, "days", 86400], // 60*60*24*7, 60*60*24
        [1209600, "Last week", "Next week"], // 60*60*24*7*4*2
        [2419200, "weeks", 604800], // 60*60*24*7*4, 60*60*24*7
        [4838400, "Last month", "Next month"], // 60*60*24*7*4*2
        [29030400, "months", 2419200], // 60*60*24*7*4*12, 60*60*24*7*4
        [58060800, "Last year", "Next year"], // 60*60*24*7*4*12*2
        [2903040000, "years", 29030400], // 60*60*24*7*4*12*100, 60*60*24*7*4*12
        [5806080000, "Last century", "Next century"], // 60*60*24*7*4*12*100*2
        [58060800000, "centuries", 2903040000] // 60*60*24*7*4*12*100*20, 60*60*24*7*4*12*100
    ];
    var seconds = (+new Date() - time) / 1000,
        token = "ago", list_choice = 1;

    if (seconds <= 60) {
        return "Just now"
    }
    //if (seconds < 0) {
    //    seconds = Math.abs(seconds);
    //    token = 'from now';
    //    list_choice = 2;
    //}
    var i = 0, format;
    while (format = time_formats[i++])
        if (seconds < format[0]) {
            if (typeof format[2] == "string")
                return format[list_choice];
            else
                return Math.floor(seconds / format[2]) + " " + format[1] + " " + token;
        }
    return time;
}
function updateNotificationCount(total) {
    $("#header_notification_bar > a > span.badge").text(total);
    var bottom = $(".notification_bottom").parent();
    if ($("#header_notification_bar a.notification-remove").length > 0) {
        bottom.appendTo("#notificaiton_list");
        bottom.show();
    } else {
        bottom.hide();
    }
    $(".first-notification-item .notification_header span").text(total);
}
function addNotification(obj) {
    var html = $(getHtmlTemplate(obj));
    html.find(".item-title").html("<i class='fa fa-bell'></i>");
    html.find("a.notification-remove").remove();
    html.find(".notification-item").attr("id", "not_pop_" + obj.NotificationId);
    $("#not_pop_" + obj.NotificationId).remove();
    $.notify(html.html(), "def");
    shake("#header_notification_bar .icon-bell");
    if (obj.State >= 2) {
        //bootstarp table refresh
    }
    GetUpdateNotification(0);
}
function shake(div) {
    var interval = 25;
    var distance = 5;
    var times = 10;
    $(div).css('position', 'relative');
    for (var iter = 0; iter < (times + 1); iter++) {
        $(div).animate({
            left: ((iter % 2 == 0 ? distance : distance * -1))
        }, interval);
    }//for                                                                                                              
    $(div).animate({ left: 0 }, interval);
}//shake 
function notificationState(state) {
    state = Number(state);
    var text_status = "notifyjs-bootstrap-def";
    if (state === 2) {
        text_status = "notifyjs-bootstrap-success";
    } else if (state === 3) {
        text_status = "notifyjs-bootstrap-error";
    }
    return text_status;
}
function notificationTimeSpan(state, dateValue) {
    var textStatus = "Started";
    if (state === 1 || state === "1") {
        textStatus = "Processing...";
    } else if (state === 2 || state === "2") {
        textStatus = "Completed";
    } else if (state === 3 || state === "3") {
        textStatus = "Failed";
    }
    return textStatus + " " + time_ago(new Date(dateValue.replace("T", " ") + " UTC"));
}
function kendohtmlDecode(value) {
    return value.replace(/&lt;/g, "<").replace(/&gt;/g, ">");
}
function notificationTitle(state) {
    switch (state) {
        case 0:
        case 1:
            return '<img src="' + BASEPATH + 'Content/img/ajax-loader.gif" />';
        case 2:
            return '<i class="fa fa-check-circle"></i>';
        case 3:
            return '<i class="fa fa-times-circle"></i>';
    }
    return "";
}
function clearNotifications() {
    bootbox.confirm({
        title: "Confirmation",
        message: "Are you sure you want to clear all completed notifications?",
        callback: function (status) {
            if (status) {
                apiCall("common/clearnotifications", "get", {}, function (data) {
                    if (data) {
                        GetUpdateNotification(0);
                        bootbox.alert({ title: "Success!", message: "All completed notifications cleared successfully." });
                    } else {
                        bootbox.alert({ title: "Warning!", message: "No completed notifications to clear." });
                    }
                });
            }
        }
    });
}
function removeNotification(notificationId) {
    bootbox.confirm({
        title: "Confirmation",
        message: "Are you sure you want to remove notification?",
        callback: function (status) {
            apiCall("common/removenotification?notificationId=" + notificationId, "get", {}, function (data) {
                if (data) {
                    GetUpdateNotification(0);
                    bootbox.alert({ title: "Success!", message: "Notification removed successfully." });
                } else {
                    bootbox.alert({ title: "Warning!", message: "No completed notifications to remove." });
                }
            });
        }
    });
}

function getMessageForNotifications(text) {
    return text;
}

function getNotificationTitle(data) {
    if (data.Message.charAt(0) === "<") {
        var a = document.createElement('a');
        a.innerHTML = data.Message;
        var innertext = a.textContent || a.innerText;
        return data.Title + " - " + innertext;
    }
    return data.Title + " - " + data.Message;
}
var mark_diagram_visible;
var mark_diagram_visible_option;
var evaluation_diagram_visible;
var evaluation_diagram_visible_option;
var questionwise_mark_diagram;
var questionwise_mark_diagram_options;
var question_vs_student_mark;
var question_vs_student_mark_options;
var noDataLoadingOption = {
    text: "No exam or data found.",
    effect: "bubble"
};
function showEChartLoading(ele, loading) {
    if (loading)
        noDataLoadingOption.text = "Loading..."
    else
        noDataLoadingOption.text = "No exam or data found.";
    ele.setOption({ noDataLoadingOption: noDataLoadingOption });
    ele.showLoading(noDataLoadingOption);
}

$(document).ready(function () {
    // Set paths
    // ------------------------------

    require.config({
        paths: {
            echarts: BASEPATH + 'assets/js/plugins/visualization/echarts'
        }
    });


    // Configuration
    // ------------------------------

    require(
        [
            'echarts',
            'echarts/theme/limitless',
            'echarts/chart/pie',
            'echarts/chart/bar',
            'echarts/chart/funnel'
        ],


        // Charts setup
        function (ec, limitless) {

            questionwise_mark_diagram = ec.init(document.getElementById('questionwise_mark_diagram'), limitless);
            question_vs_student_mark = ec.init(document.getElementById('question_vs_student_mark'), limitless);
            mark_diagram_visible = ec.init(document.getElementById('mark_diagram_visible'), limitless);
            evaluation_diagram_visible = ec.init(document.getElementById('evaluation_diagram_visible'), limitless);

            questionwise_mark_diagram_options = {
                noDataLoadingOption: noDataLoadingOption,

                // Display toolbox
                toolbox: {
                    show: true,
                    orient: 'vertical',
                    feature: {
                        magicType: {
                            show: true,
                            title: {
                                stack: 'Switch to stack chart',
                                tiled: 'Switch to tiled chart',
                            },
                            type: ['stack', 'tiled']
                        },
                        restore: {
                            show: true,
                            title: 'Restore'
                        },
                        saveAsImage: {
                            show: true,
                            title: 'Same as image',
                            lang: ['Save']
                        }
                    }
                },

                // Add tooltip
                tooltip: {
                    trigger: 'item',
                    formatter: function (params) {
                        return params.name + "<br/>" + params.data.title + "<br/>" + params.seriesName + ": " + params.value.toFixed(2);
                    },
                    transitionDuration: 0,
                    axisPointer: {
                        type: 'shadow'
                    }
                },

                // Add legend
                legend: {
                    data: ['Avg. Marks', 'Total Marks']
                },

                // Enable drag recalculate
                calculable: true,

                // Horizontal axis
                xAxis: [{
                    type: 'value',
                    boundaryGap: [0, 0.05]
                }],

                // Vertical axis
                yAxis: [{
                    type: 'category',
                    //data: ['Q1', 'Q2', 'Q3', 'Q4', 'Q5']
                    data: []
                }],

                // Add series
                series: [
                    {
                        name: 'Avg. Marks',
                        type: 'bar',
                        itemStyle: {
                            normal: {
                                color: '#EF5350'
                            }
                        },
                        //data: [2, 6, 5, 5, 9]
                        data: []
                    },
                    {
                        name: 'Total Marks',
                        type: 'bar',
                        itemStyle: {
                            normal: {
                                color: '#66BB6A'
                            }
                        },
                        //data: [2, 4, 4, 8, 7]
                        data: []
                    }
                ]
            };

            question_vs_student_mark_options = {
                noDataLoadingOption: noDataLoadingOption,

                // Setup grid
                grid: {
                    x: 75,
                    x2: 25,
                    y: 35,
                    y2: 25
                },

                // Display toolbox
                toolbox: {
                    show: true,
                    orient: 'vertical',
                    feature: {
                        magicType: {
                            show: true,
                            title: {
                                stack: 'Switch to stack chart',
                                tiled: 'Switch to tiled chart',
                            },
                            type: ['stack', 'tiled']
                        },
                        restore: {
                            show: true,
                            title: 'Restore'
                        },
                        saveAsImage: {
                            show: true,
                            title: 'Same as image',
                            lang: ['Save']
                        }
                    }
                },

                // Add tooltip
                tooltip: {
                    trigger: 'axis',
                    transitionDuration: 0,
                    formatter: function (params) {
                        var tooltipText = "";
                        var total = 0;
                        if (params.length > 0) {
                            tooltipText = params[0].name + "<br/>" + params[0].data.title + "<br/>";
                        }
                        $(params).each(function (i, d) {
                            tooltipText += d.seriesName + ": " + d.value + " Student(s)<br/>";
                            total += d.value;
                        });
                        return tooltipText + "Total Students: " + total + "<br/>" + "Avg. Of Students: " + (total / (params.length == 0 ? 1 : params.length)).toFixed(2);
                    },
                    axisPointer: {
                        type: 'shadow'
                    }
                },

                // Add legend
                legend: {
                    data: []
                },

                // Enable drag recalculate
                calculable: true,

                // Horizontal axis
                xAxis: [{
                    type: 'value'
                }],

                // Vertical axis
                yAxis: [{
                    type: 'category',
                    data: []
                }],

                // Add series
                series: []
            };

            mark_diagram_visible_option = {
                noDataLoadingOption: noDataLoadingOption,
                // Add tooltip
                tooltip: {
                    trigger: 'item',
                    transitionDuration: 0,
                    formatter: function (params) {
                        return params.data.name + " - Questions<br/>Total: " + params.data.title + "<br/>" + "Evaluated: " + params.value + "<br/>Progress: " + (Number(params.value) * 100 / Number(params.data.title)).toFixed(2) + "%";
                    }
                },

                // Add legend
                legend: {
                    name: "ABC",
                    x: 'left',
                    y: 'top',
                    orient: 'vertical',
                    //data: ['Exam 1', 'Exam 2', 'Exam 3', 'Exam 4', 'Exam 5']
                    data: []
                },

                // Display toolbox
                toolbox: {
                    show: true,
                    orient: 'vertical',
                    feature: {
                        magicType: {
                            show: true,
                            title: {
                                pie: 'Switch to pies',
                                funnel: 'Switch to funnel',
                            },
                            type: ['pie', 'funnel']
                        },
                        restore: {
                            show: true,
                            title: 'Restore'
                        },
                        saveAsImage: {
                            show: true,
                            title: 'Same as image',
                            lang: ['Save']
                        }
                    }
                },

                // Enable drag recalculate
                calculable: true,

                // Add series
                series: [
                    {
                        name: '-',
                        type: 'pie',
                        radius: ['15%', '73%'],
                        center: ['50%', '57%'],
                        roseType: 'area',

                        // Funnel
                        width: '40%',
                        height: '78%',
                        x: '30%',
                        y: '17.5%',
                        max: 450,
                        sort: 'ascending',

                        /*data: [
                            { value: 4, name: 'Exam 1', title: 'Title1' },
                            { value: 6, name: 'Exam 2', title: 'Title1' },
                            { value: 5, name: 'Exam 3', title: 'Title1' },
                            { value: 2, name: 'Exam 4', title: 'Title1' },
                            { value: 9, name: 'Exam 5', title: 'Title1' }
                        ]*/
                        data: []
                    }
                ]
            };

            evaluation_diagram_visible_option = {
                noDataLoadingOption: noDataLoadingOption,

                // Add tooltip
                tooltip: {
                    trigger: 'item',
                    transitionDuration: 0,
                    formatter: function (params) {
                        return params.data.name + " (" + params.data.title + "):<br/>Total :" + params.data.total + "<br/>" + "Evaluated: " + params.value + "<br/>Progress: " + (Number(params.value) * 100 / Number(params.data.total)).toFixed(2) + "%";
                    }
                },

                // Add legend
                legend: {
                    x: 'left',
                    y: 'top',
                    orient: 'vertical',
                    //data: ['Exam 1', 'Exam 2', 'Exam 3', 'Exam 4', 'Exam 5', 'Exam 6', 'Exam 7', 'Exam 8']
                    data: []
                },

                // Display toolbox
                toolbox: {
                    show: true,
                    orient: 'vertical',
                    feature: {
                        magicType: {
                            show: true,
                            title: {
                                pie: 'Switch to pies',
                                funnel: 'Switch to funnel',
                            },
                            type: ['pie', 'funnel']
                        },
                        restore: {
                            show: true,
                            title: 'Restore'
                        },
                        saveAsImage: {
                            show: true,
                            title: 'Same as image',
                            lang: ['Save']
                        }
                    }
                },

                // Enable drag recalculate
                calculable: true,

                // Add series
                series: [
                    {
                        name: '-',
                        type: 'pie',
                        radius: ['15%', '73%'],
                        center: ['50%', '57%'],
                        roseType: 'area',

                        // Funnel
                        width: '40%',
                        height: '78%',
                        x: '30%',
                        y: '17.5%',
                        max: 450,
                        sort: 'ascending',

                        /*data: [
                            { value: 3, name: 'Exam 1', title: 'Title1' },
                            { value: 7, name: 'Exam 2', title: 'Title1' },
                            { value: 4, name: 'Exam 3', title: 'Title1' },
                            { value: 9, name: 'Exam 4', title: 'Title1' },
                            { value: 10, name: 'Exam 5', title: 'Title1' },
                            { value: 3, name: 'Exam 6', title: 'Title1' },
                            { value: 5, name: 'Exam 7', title: 'Title1' },
                            { value: 4, name: 'Exam 8', title: 'Title1' }
                        ]*/
                        data: []
                    }
                ]
            };

            questionwise_mark_diagram.setOption(questionwise_mark_diagram_options);
            question_vs_student_mark.setOption(question_vs_student_mark_options);
            mark_diagram_visible.setOption(mark_diagram_visible_option);
            evaluation_diagram_visible.setOption(evaluation_diagram_visible_option);

            window.onresize = function () {
                setTimeout(function () {
                    questionwise_mark_diagram.resize();
                    question_vs_student_mark.resize();
                    mark_diagram_visible.resize();
                    evaluation_diagram_visible.resize();
                }, 200);
            }

            BindCharts();
        }
    );
    BindDashboard();
});
var grid_evaluationsummary_search = null;
//var grid_evaluationsummary_timer = null;
//var grid_studentsummary_search = null;
//var grid_studentsummary_timer = null;
function BindDashboard() {
    SHOW_NP_PROGRESS = false;
    $("#grid_evaluationsummary").on("load-success.bs.table", function (e, data) {
        grid_evaluationsummary_search = data.CurrentSearch;
        grid_evaluationsummary_search["total"] = data.total;
        //if (grid_evaluationsummary_timer != null)
        //    clearInterval(grid_evaluationsummary_timer);
        //grid_evaluationsummary_timer = setInterval(function () {
        //    $("#grid_evaluationsummary").bootstrapTable('refresh', { params: grid_evaluationsummary_search });
        //}, 5000)
    });

    $("#grid_studentevaluation").on("load-success.bs.table", function (e, data) {
        grid_studentsummary_search = data.CurrentSearch;
        //if (grid_evaluationsummary_timer != null)
        //    clearInterval(grid_studentsummary_timer);
        //grid_studentsummary_timer = setInterval(function () {
        //    $("#grid_studentevaluation").bootstrapTable('refresh', { params: grid_studentsummary_search });
        //}, 6000)
    }).on("expand-row.bs.table", function (e, index, row, $detail) {
        $detail.html('<div class="ml-20 mr-20"><table class="table table-bordered"></table></div>').find('table').bootstrapTable({
            columns: [{
                field: 'QuestionNo',
                title: 'No.'
            }, {
                field: 'QustionTitle',
                title: 'Qustion Title'
            }, {
                field: 'QuestionMark',
                title: 'Question Mark'
            }, {
                field: 'ObtainMark',
                title: 'Obtain Mark'
            }],
            data: row.QuestionSummary,
        });
    });
}
function BindCharts() {
    //First
    $("#questionwisemarkpaperid").on("change", function () {
        showEChartLoading(questionwise_mark_diagram, true);
        apiCall("dashboard/getaveragescoreevaluation?paperId=" + $(this).val(), "get", {}, function (data) {
            questionwise_mark_diagram.clear();
            if (data.StatusCode == 200 && data.Data.length > 0) {
                questionwise_mark_diagram_options.yAxis[0].data = [];
                questionwise_mark_diagram_options.series[0].data = [];
                questionwise_mark_diagram_options.series[1].data = [];
                $(data.Data).each(function (i, item) {
                    questionwise_mark_diagram_options.yAxis[0].data.push("Q." + (i + 1));
                    questionwise_mark_diagram_options.series[0].data.push({ value: item.AverageMark, title: item.QuestionTitle });
                    questionwise_mark_diagram_options.series[1].data.push({ value: item.TotalQuestionMark, title: item.QuestionTitle });
                });
                questionwise_mark_diagram.hideLoading();
            } else {
                questionwise_mark_diagram_options.yAxis[0].data = [];
                questionwise_mark_diagram_options.series[0].data = [];
                questionwise_mark_diagram_options.series[1].data = [];
                showEChartLoading(questionwise_mark_diagram, false);
            }
            questionwise_mark_diagram.setOption(questionwise_mark_diagram_options);
        });
    });
    if ($("#questionwisemarkpaperid option").length > 0) {
        $("#questionwisemarkpaperid").val($("#questionwisemarkpaperid option:first").next().attr("value"));
        $("#questionwisemarkpaperid").change();
    }

    //Second
    $("#question_vs_student_visible_paperid").on("change", function () {
        showEChartLoading(question_vs_student_mark, true);
        apiCall("dashboard/getquestionvssstudentmark?paperId=" + $(this).val(), "get", {}, function (data) {
            question_vs_student_mark.clear();
            if (data.StatusCode == 200 && data.Data) {
                question_vs_student_mark_options.legend.data = [];
                question_vs_student_mark_options.yAxis[0].data = [];
                question_vs_student_mark_options.series = [];

                $(data.Data.YAxis).each(function (i, item) {
                    question_vs_student_mark_options.yAxis[0].data.push("Q." + item);
                });

                $(data.Data.Marks).each(function (i, item) {
                    question_vs_student_mark_options.legend.data.push(item.Label + " Score");
                    var series = {
                        name: item.Label + " Score",
                        type: 'bar',
                        stack: 'Total',
                        itemStyle: {
                            normal: {
                                label: {
                                    show: true,
                                    position: 'inside'
                                }
                            },
                            emphasis: {
                                label: {
                                    show: true
                                }
                            }
                        },
                        data: []
                    };
                    $(item.Details).each(function (j, subItem) {
                        series.data.push({ value: subItem.TotalStudents, title: subItem.QTitle });
                    });
                    question_vs_student_mark_options.series.push(series);
                });
                question_vs_student_mark.hideLoading();
            } else {
                question_vs_student_mark_options.legend.data = [];
                question_vs_student_mark_options.series = [];
                question_vs_student_mark_options.yAxis[0].data = [];
                showEChartLoading(question_vs_student_mark, false);
            }
            question_vs_student_mark.setOption(question_vs_student_mark_options);
        });
    });

    if ($("#question_vs_student_visible_paperid option").length > 0) {
        $("#question_vs_student_visible_paperid").val($("#question_vs_student_visible_paperid option:first").next().attr("value"));
        $("#question_vs_student_visible_paperid").change();
    }

    //Third

    apiCall("dashboard/getrecentcomletedexam", "get", {}, function (data) {
        mark_diagram_visible.clear();
        if (data.StatusCode == 200 && data.Data.length > 0) {
            mark_diagram_visible_option.series[0].name = "Total vs Evaluated Questions";
            mark_diagram_visible_option.legend.data = [];
            mark_diagram_visible_option.series[0].data = [];
            $(data.Data).each(function (i, item) {
                mark_diagram_visible_option.legend.data.push(item.ExamTitle);
                mark_diagram_visible_option.series[0].data.push({ value: item.TotalEvaluated, name: item.ExamTitle, title: item.TotalQuestion });
            });
            mark_diagram_visible.hideLoading();
        } else {
            mark_diagram_visible_option.series[0].name = "-";
            mark_diagram_visible_option.legend.data = [];
            mark_diagram_visible_option.series[0].data = [];
            showEChartLoading(mark_diagram_visible, false);
        }
        mark_diagram_visible.setOption(mark_diagram_visible_option);
    });

    //Four

    $("#evaluation_diagram_visible_paperid").on("change", function () {
        showEChartLoading(evaluation_diagram_visible, true);
        apiCall("dashboard/getrecentevaluatedexam?paperId=" + $(this).val(), "get", {}, function (data) {
            evaluation_diagram_visible.clear();
            if (data.StatusCode == 200 && data.Data.length > 0) {
                evaluation_diagram_visible_option.series[0].name = data.Data[0].ExamTitle + " (exam) evaluator wise average score";
                evaluation_diagram_visible_option.legend.data = [];
                evaluation_diagram_visible_option.series[0].data = [];
                $(data.Data).each(function (i, item) {
                    evaluation_diagram_visible_option.legend.data.push(item.Evaluator);
                    evaluation_diagram_visible_option.series[0].data.push({ value: item.EvaluatedQuestion, name: item.Evaluator, title: item.EvaluatorEmail, total: item.TotalEvalQuestion });
                });
                evaluation_diagram_visible.hideLoading();
            } else {
                evaluation_diagram_visible_option.series[0].name = "-";
                evaluation_diagram_visible_option.legend.data = [];
                evaluation_diagram_visible_option.series[0].data = [];
                showEChartLoading(evaluation_diagram_visible, false);
            }
            evaluation_diagram_visible.setOption(evaluation_diagram_visible_option);
        });
    });
    if ($("#evaluation_diagram_visible_paperid option").length > 0) {
        $("#evaluation_diagram_visible_paperid").val($("#evaluation_diagram_visible_paperid option:first").next().attr("value"));
        $("#evaluation_diagram_visible_paperid").change();
    }
}

function downloadEvaluationExcel() {
    if (grid_evaluationsummary_search != null && grid_evaluationsummary_search.total > 0) {
        SHOW_NP_PROGRESS = true;
        apiCall("dashboard/evaluationsummaryexport", "post", grid_evaluationsummary_search, function (data) {
            NPStop();
            SHOW_NP_PROGRESS = false;
            if (data.StatusCode === 200) {
                window.location = ApiUrl + "common/downloadexcel?fileName=" + data.Data;
            }
        }, function () {
            NPStop();
            SHOW_NP_PROGRESS = false;
        });
    } else {
        AlertNotify("Evaluation Summary", "No matching records found to export", "bg-warning");
    }
}

function downloadStudQSummaryExcel() {
    var paperId = $.trim($("#studentsummayreport").val());
    if (paperId != "") {
        SHOW_NP_PROGRESS = true;
        apiCall("dashboard/questionssummaryreport?paperId=" + paperId, "post", {}, function (data) {
            NPStop();
            SHOW_NP_PROGRESS = false;
            if (data.StatusCode === 200) {
                window.location = ApiUrl + "common/downloadexcel?fileName=" + data.Data;
            }
        }, function () {
            NPStop();
            SHOW_NP_PROGRESS = false;
        });
    } else {
        AlertNotify("Student vs Question Summary", "Please select exam", "bg-warning");
    }
}

$("#studentsummayreport").on("change", function () {
    var paperId = $(this).val();
    SHOW_NP_PROGRESS = true;
    apiCall("dashboard/studentsummaryreportquestions?paperId=" + paperId, "get", {}, function (data) {
        var totalquestions = data.Data;
        if (data.StatusCode == 200) {
            $.get(BASEPATH + "Home/StudentSummary?paperId=" + paperId + "&total=" + totalquestions).done(function (html) {
                NPStop();
                SHOW_NP_PROGRESS = false;
                $("#stud_vs_q_summary").html(html);
            });
        }
    });
});
//function downloadStudentExcel(){
//    apiCall("dashboard/studentsummaryexport", "post", grid_studentsummary_search, function (data) {
//        if (data.StatusCode === 200) {
//            window.location = ApiUrl + "common/downloadexcel?fileName=" + data.Data;
//        }
//    });
//}

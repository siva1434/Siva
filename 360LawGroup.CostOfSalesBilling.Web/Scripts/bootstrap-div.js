(function (h, q) {
    h.fn.bootpag = function (p) {
        function m(c, b) {
            b = parseInt(b, 10); var d, e = 0 == a.maxVisible ? 1 : a.maxVisible, k = 1 == a.maxVisible ? 0 : 1, n = Math.floor((b - 1) / e) * e, f = c.find("li"); a.page = b = 0 > b ? 0 : b > a.total ? a.total : b; f.removeClass(a.activeClass); d = 1 > b - 1 ? 1 : a.leaps && b - 1 >= a.maxVisible ? Math.floor((b - 1) / e) * e : b - 1; a.firstLastUse && f.first().toggleClass(a.disabledClass, 1 === b); e = f.first(); a.firstLastUse && (e = e.next()); e.toggleClass(a.disabledClass, 1 === b).attr("data-lp", d).find("a").attr("href", g(d)); k = 1 == a.maxVisible ?
                0 : 1; d = b + 1 > a.total ? a.total : a.leaps && b + 1 < a.total - a.maxVisible ? n + a.maxVisible + k : b + 1; e = f.last(); a.firstLastUse && (e = e.prev()); e.toggleClass(a.disabledClass, b === a.total).attr("data-lp", d).find("a").attr("href", g(d)); f.last().toggleClass(a.disabledClass, b === a.total); e = f.filter("[data-lp=" + b + "]"); k = "." + [a.nextClass, a.prevClass, a.firstClass, a.lastClass].join(",."); if (!e.not(k).length) {
                    var m = b <= n ? -a.maxVisible : 0; f.not(k).each(function (b) {
                        d = b + 1 + n + m; h(this).attr("data-lp", d).toggle(d <= a.total).find("a").html(d).attr("href",
                            g(d));
                    }); e = f.filter("[data-lp=" + b + "]");
                } e.not(k).addClass(a.activeClass); l.data("settings", a);
        } function g(c) { return a.href.replace(a.hrefVariable, c) } var l = this, a = h.extend({
            total: 0, page: 1, maxVisible: null, leaps: !0, href: "javascript:void(0);", hrefVariable: "{{number}}", next: "&raquo;", prev: "&laquo;", firstLastUse: !1, first: '<span aria-hidden="true">&larr;</span>', last: '<span aria-hidden="true">&rarr;</span>', wrapClass: "pagination", activeClass: "active", disabledClass: "disabled", nextClass: "next", prevClass: "prev",
            lastClass: "last", firstClass: "first"
        }, l.data("settings") || {}, p || {}); if (0 >= a.total) return this; h.isNumeric(a.maxVisible) || a.maxVisible || (a.maxVisible = parseInt(a.total, 10)); l.data("settings", a); return this.each(function () {
            var c, b, d = h(this); c = ['<ul class="', a.wrapClass, ' bootpag">']; a.firstLastUse && (c = c.concat(['<li data-lp="1" class="', a.firstClass, '"><a href="', g(1), '">', a.first, "</a></li>"])); a.prev && (c = c.concat(['<li data-lp="1" class="', a.prevClass, '"><a href="', g(1), '">', a.prev, "</a></li>"])); for (b =
                1; b <= Math.min(a.total, a.maxVisible); b++) c = c.concat(['<li data-lp="', b, '"><a href="', g(b), '">', b, "</a></li>"]); a.next && (b = a.leaps && a.total > a.maxVisible ? Math.min(a.maxVisible + 1, a.total) : 2, c = c.concat(['<li data-lp="', b, '" class="', a.nextClass, '"><a href="', g(b), '">', a.next, "</a></li>"])); a.firstLastUse && (c = c.concat(['<li data-lp="', a.total, '" class="last"><a href="', g(a.total), '">', a.last, "</a></li>"])); c.push("</ul>"); d.find("ul.bootpag").remove(); d.append(c.join("")); c = d.find("ul.bootpag"); d.find("ul.bootpag > li").click(function () {
                    var b =
                        h(this); if (!b.hasClass(a.disabledClass) && !b.hasClass(a.activeClass)) { var c = parseInt(b.attr("data-lp"), 10); l.find("ul.bootpag").each(function () { m(h(this), c) }); l.trigger("page", c) }
                }); m(c, a.page);
        });
    };
})(jQuery, window);

(function (jQuery, undefined) {
    var oldManip = jQuery.fn.domManip, tmplItmAtt = "_tmplitem", htmlExpr = /^[^<]*(<[\w\W]+>)[^>]*$|\{\{\! /,
        newTmplItems = {}, wrappedItems = {}, appendToTmplItems, topTmplItem = { key: 0, data: {} }, itemKey = 0, cloneIndex = 0, stack = [];

    function newTmplItem(options, parentItem, fn, data) {
        // Returns a template item data structure for a new rendered instance of a template (a 'template item').
        // The content field is a hierarchical array of strings and nested items (to be
        // removed and replaced by nodes field of dom elements, once inserted in DOM).
        var newItem = {
            data: data || (parentItem ? parentItem.data : {}),
            _wrap: parentItem ? parentItem._wrap : null,
            tmpl: null,
            parent: parentItem || null,
            nodes: [],
            calls: tiCalls,
            nest: tiNest,
            wrap: tiWrap,
            html: tiHtml,
            update: tiUpdate
        };
        if (options) {
            jQuery.extend(newItem, options, { nodes: [], parent: parentItem });
        }
        if (fn) {
            // Build the hierarchical content to be used during insertion into DOM
            newItem.tmpl = fn;
            newItem._ctnt = newItem._ctnt || newItem.tmpl(jQuery, newItem);
            newItem.key = ++itemKey;
            // Keep track of new template item, until it is stored as jQuery Data on DOM element
            (stack.length ? wrappedItems : newTmplItems)[itemKey] = newItem;
        }
        return newItem;
    }

    // Override appendTo etc., in order to provide support for targeting multiple elements. (This code would disappear if integrated in jquery core).
    jQuery.each({
        appendTo: "append",
        prependTo: "prepend",
        insertBefore: "before",
        insertAfter: "after",
        replaceAll: "replaceWith"
    }, function (name, original) {
        jQuery.fn[name] = function (selector) {
            var ret = [], insert = jQuery(selector), elems, i, l, tmplItems,
                parent = this.length === 1 && this[0].parentNode;

            appendToTmplItems = newTmplItems || {};
            if (parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1) {
                insert[original](this[0]);
                ret = this;
            } else {
                for (i = 0, l = insert.length; i < l; i++) {
                    cloneIndex = i;
                    elems = (i > 0 ? this.clone(true) : this).get();
                    jQuery.fn[original].apply(jQuery(insert[i]), elems);
                    ret = ret.concat(elems);
                }
                cloneIndex = 0;
                ret = this.pushStack(ret, name, insert.selector);
            }
            tmplItems = appendToTmplItems;
            appendToTmplItems = null;
            jQuery.tmpl.complete(tmplItems);
            return ret;
        };
    });

    jQuery.fn.extend({
        // Use first wrapped element as template markup.
        // Return wrapped set of template items, obtained by rendering template against data.
        tmpl: function (data, options, parentItem) {
            return jQuery.tmpl(this[0], data, options, parentItem);
        },

        // Find which rendered template item the first wrapped DOM element belongs to
        tmplItem: function () {
            return jQuery.tmplItem(this[0]);
        },

        // Consider the first wrapped element as a template declaration, and get the compiled template or store it as a named template.
        template: function (name) {
            return jQuery.template(name, this[0]);
        },

        domManip: function (args, table, callback, options) {
            // This appears to be a bug in the appendTo, etc. implementation
            // it should be doing .call() instead of .apply(). See #6227
            if (args[0] && args[0].nodeType) {
                var dmArgs = jQuery.makeArray(arguments), argsLength = args.length, i = 0, tmplItem;
                while (i < argsLength && !(tmplItem = jQuery.data(args[i++], "tmplItem"))) { }
                if (argsLength > 1) {
                    dmArgs[0] = [jQuery.makeArray(args)];
                }
                if (tmplItem && cloneIndex) {
                    dmArgs[2] = function (fragClone) {
                        // Handler called by oldManip when rendered template has been inserted into DOM.
                        jQuery.tmpl.afterManip(this, fragClone, callback);
                    };
                }
                oldManip.apply(this, dmArgs);
            } else {
                oldManip.apply(this, arguments);
            }
            cloneIndex = 0;
            if (!appendToTmplItems) {
                jQuery.tmpl.complete(newTmplItems);
            }
            return this;
        }
    });

    jQuery.extend({
        // Return wrapped set of template items, obtained by rendering template against data.
        tmpl: function (tmpl, data, options, parentItem) {
            var ret, topLevel = !parentItem;
            if (topLevel) {
                // This is a top-level tmpl call (not from a nested template using {{tmpl}})
                parentItem = topTmplItem;
                tmpl = jQuery.template[tmpl] || jQuery.template(null, tmpl);
                wrappedItems = {}; // Any wrapped items will be rebuilt, since this is top level
            } else if (!tmpl) {
                // The template item is already associated with DOM - this is a refresh.
                // Re-evaluate rendered template for the parentItem
                tmpl = parentItem.tmpl;
                newTmplItems[parentItem.key] = parentItem;
                parentItem.nodes = [];
                if (parentItem.wrapped) {
                    updateWrapped(parentItem, parentItem.wrapped);
                }
                // Rebuild, without creating a new template item
                return jQuery(build(parentItem, null, parentItem.tmpl(jQuery, parentItem)));
            }
            if (!tmpl) {
                return []; // Could throw...
            }
            if (typeof data === "function") {
                data = data.call(parentItem || {});
            }
            if (options && options.wrapped) {
                updateWrapped(options, options.wrapped);
            }
            ret = jQuery.isArray(data) ?
                jQuery.map(data, function (dataItem) {
                    return dataItem ? newTmplItem(options, parentItem, tmpl, dataItem) : null;
                }) :
                [newTmplItem(options, parentItem, tmpl, data)];
            return topLevel ? jQuery(build(parentItem, null, ret)) : ret;
        },

        // Return rendered template item for an element.
        tmplItem: function (elem) {
            var tmplItem;
            if (elem instanceof jQuery) {
                elem = elem[0];
            }
            while (elem && elem.nodeType === 1 && !(tmplItem = jQuery.data(elem, "tmplItem")) && (elem = elem.parentNode)) { }
            return tmplItem || topTmplItem;
        },

        // Set:
        // Use $.template( name, tmpl ) to cache a named template,
        // where tmpl is a template string, a script element or a jQuery instance wrapping a script element, etc.
        // Use $( "selector" ).template( name ) to provide access by name to a script block template declaration.

        // Get:
        // Use $.template( name ) to access a cached template.
        // Also $( selectorToScriptBlock ).template(), or $.template( null, templateString )
        // will return the compiled template, without adding a name reference.
        // If templateString includes at least one HTML tag, $.template( templateString ) is equivalent
        // to $.template( null, templateString )
        template: function (name, tmpl) {
            if (tmpl) {
                // Compile template and associate with name
                if (typeof tmpl === "string") {
                    // This is an HTML string being passed directly in.
                    tmpl = buildTmplFn(tmpl)
                } else if (tmpl instanceof jQuery) {
                    tmpl = tmpl[0] || {};
                }
                if (tmpl.nodeType) {
                    // If this is a template block, use cached copy, or generate tmpl function and cache.
                    tmpl = jQuery.data(tmpl, "tmpl") || jQuery.data(tmpl, "tmpl", buildTmplFn(tmpl.innerHTML));
                }
                return typeof name === "string" ? (jQuery.template[name] = tmpl) : tmpl;
            }
            // Return named compiled template
            return name ? (typeof name !== "string" ? jQuery.template(null, name) :
                (jQuery.template[name] ||
                    // If not in map, treat as a selector. (If integrated with core, use quickExpr.exec) 
                    jQuery.template(null, htmlExpr.test(name) ? name : jQuery(name)))) : null;
        },

        encode: function (text) {
            // Do HTML encoding replacing < > & and ' and " by corresponding entities.
            return ("" + text).split("<").join("&lt;").split(">").join("&gt;").split('"').join("&#34;").split("'").join("&#39;");
        }
    });

    jQuery.extend(jQuery.tmpl, {
        tag: {
            "tmpl": {
                _default: { $2: "null" },
                open: "if($notnull_1){_=_.concat($item.nest($1,$2));}"
                // tmpl target parameter can be of type function, so use $1, not $1a (so not auto detection of functions)
                // This means that {{tmpl foo}} treats foo as a template (which IS a function). 
                // Explicit parens can be used if foo is a function that returns a template: {{tmpl foo()}}.
            },
            "wrap": {
                _default: { $2: "null" },
                open: "$item.calls(_,$1,$2);_=[];",
                close: "call=$item.calls();_=call._.concat($item.wrap(call,_));"
            },
            "each": {
                _default: { $2: "$index, $value" },
                open: "if($notnull_1){$.each($1a,function($2){with(this){",
                close: "}});}"
            },
            "if": {
                open: "if(($notnull_1) && $1a){",
                close: "}"
            },
            "else": {
                _default: { $1: "true" },
                open: "}else if(($notnull_1) && $1a){"
            },
            "html": {
                // Unecoded expression evaluation. 
                open: "if($notnull_1){_.push($1a);}"
            },
            "=": {
                // Encoded expression evaluation. Abbreviated form is ${}.
                _default: { $1: "$data" },
                open: "if($notnull_1){_.push($.encode($1a));}"
            },
            "!": {
                // Comment tag. Skipped by parser
                open: ""
            }
        },

        // This stub can be overridden, e.g. in jquery.tmplPlus for providing rendered events
        complete: function (items) {
            newTmplItems = {};
        },

        // Call this from code which overrides domManip, or equivalent
        // Manage cloning/storing template items etc.
        afterManip: function afterManip(elem, fragClone, callback) {
            // Provides cloned fragment ready for fixup prior to and after insertion into DOM
            var content = fragClone.nodeType === 11 ?
                jQuery.makeArray(fragClone.childNodes) :
                fragClone.nodeType === 1 ? [fragClone] : [];

            // Return fragment to original caller (e.g. append) for DOM insertion
            callback.call(elem, fragClone);

            // Fragment has been inserted:- Add inserted nodes to tmplItem data structure. Replace inserted element annotations by jQuery.data.
            storeTmplItems(content);
            cloneIndex++;
        }
    });

    //========================== Private helper functions, used by code above ==========================

    function build(tmplItem, nested, content) {
        // Convert hierarchical content into flat string array 
        // and finally return array of fragments ready for DOM insertion
        var frag, ret = content ? jQuery.map(content, function (item) {
            return (typeof item === "string") ?
                // Insert template item annotations, to be converted to jQuery.data( "tmplItem" ) when elems are inserted into DOM.
                (tmplItem.key ? item.replace(/(<\w+)(?=[\s>])(?![^>]*_tmplitem)([^>]*)/g, "$1 " + tmplItmAtt + "=\"" + tmplItem.key + "\" $2") : item) :
                // This is a child template item. Build nested template.
                build(item, tmplItem, item._ctnt);
        }) :
            // If content is not defined, insert tmplItem directly. Not a template item. May be a string, or a string array, e.g. from {{html $item.html()}}. 
            tmplItem;
        if (nested) {
            return ret;
        }

        // top-level template
        ret = ret.join("");

        // Support templates which have initial or final text nodes, or consist only of text
        // Also support HTML entities within the HTML markup.
        ret.replace(/^\s*([^<\s][^<]*)?(<[\w\W]+>)([^>]*[^>\s])?\s*$/, function (all, before, middle, after) {
            frag = jQuery(middle).get();

            storeTmplItems(frag);
            if (before) {
                frag = unencode(before).concat(frag);
            }
            if (after) {
                frag = frag.concat(unencode(after));
            }
        });
        return frag ? frag : unencode(ret);
    }

    function unencode(text) {
        // Use createElement, since createTextNode will not render HTML entities correctly
        var el = document.createElement("div");
        el.innerHTML = text;
        return jQuery.makeArray(el.childNodes);
    }

    // Generate a reusable function that will serve to render a template against data
    function buildTmplFn(markup) {
        return new Function("jQuery", "$item",
            "var $=jQuery,call,_=[],$data=$item.data;" +

            // Introduce the data as local variables using with(){}
            "with($data){_.push('" +

            // Convert the template into pure JavaScript
            jQuery.trim(markup)
                .replace(/([\\'])/g, "\\$1")
                .replace(/[\r\t\n]/g, " ")
                .replace(/\$\{([^\}]*)\}/g, "{{= $1}}")
                .replace(/\{\{(\/?)(\w+|.)(?:\(((?:[^\}]|\}(?!\}))*?)?\))?(?:\s+(.*?)?)?(\(((?:[^\}]|\}(?!\}))*?)\))?\s*\}\}/g,
                function (all, slash, type, fnargs, target, parens, args) {
                    var tag = jQuery.tmpl.tag[type], def, expr, exprAutoFnDetect;
                    if (!tag) {
                        throw "Template command not found: " + type;
                    }
                    def = tag._default || [];
                    if (parens && !/\w$/.test(target)) {
                        target += parens;
                        parens = "";
                    }
                    if (target) {
                        target = unescape(target);
                        args = args ? ("," + unescape(args) + ")") : (parens ? ")" : "");
                        // Support for target being things like a.toLowerCase();
                        // In that case don't call with template item as 'this' pointer. Just evaluate...
                        expr = parens ? (target.indexOf(".") > -1 ? target + parens : ("(" + target + ").call($item" + args)) : target;
                        exprAutoFnDetect = parens ? expr : "(typeof(" + target + ")==='function'?(" + target + ").call($item):(" + target + "))";
                    } else {
                        exprAutoFnDetect = expr = def.$1 || "null";
                    }
                    fnargs = unescape(fnargs);
                    return "');" +
                        tag[slash ? "close" : "open"]
                            .split("$notnull_1").join(target ? "typeof(" + target + ")!=='undefined' && (" + target + ")!=null" : "true")
                            .split("$1a").join(exprAutoFnDetect)
                            .split("$1").join(expr)
                            .split("$2").join(fnargs ?
                                fnargs.replace(/\s*([^\(]+)\s*(\((.*?)\))?/g, function (all, name, parens, params) {
                                    params = params ? ("," + params + ")") : (parens ? ")" : "");
                                    return params ? ("(" + name + ").call($item" + params) : all;
                                })
                                : (def.$2 || "")
                            ) +
                        "_.push('";
                }) +
            "');}return _;"
        );
    }
    function updateWrapped(options, wrapped) {
        // Build the wrapped content. 
        options._wrap = build(options, true,
            // Suport imperative scenario in which options.wrapped can be set to a selector or an HTML string.
            jQuery.isArray(wrapped) ? wrapped : [htmlExpr.test(wrapped) ? wrapped : jQuery(wrapped).html()]
        ).join("");
    }

    function unescape(args) {
        return args ? args.replace(/\\'/g, "'").replace(/\\\\/g, "\\") : null;
    }
    function outerHtml(elem) {
        var div = document.createElement("div");
        div.appendChild(elem.cloneNode(true));
        return div.innerHTML;
    }

    // Store template items in jQuery.data(), ensuring a unique tmplItem data data structure for each rendered template instance.
    function storeTmplItems(content) {
        var keySuffix = "_" + cloneIndex, elem, elems, newClonedItems = {}, i, l, m;
        for (i = 0, l = content.length; i < l; i++) {
            if ((elem = content[i]).nodeType !== 1) {
                continue;
            }
            elems = elem.getElementsByTagName("*");
            for (m = elems.length - 1; m >= 0; m--) {
                processItemKey(elems[m]);
            }
            processItemKey(elem);
        }
        function processItemKey(el) {
            var pntKey, pntNode = el, pntItem, tmplItem, key;
            // Ensure that each rendered template inserted into the DOM has its own template item,
            if ((key = el.getAttribute(tmplItmAtt))) {
                while (pntNode.parentNode && (pntNode = pntNode.parentNode).nodeType === 1 && !(pntKey = pntNode.getAttribute(tmplItmAtt))) { }
                if (pntKey !== key) {
                    // The next ancestor with a _tmplitem expando is on a different key than this one.
                    // So this is a top-level element within this template item
                    // Set pntNode to the key of the parentNode, or to 0 if pntNode.parentNode is null, or pntNode is a fragment.
                    pntNode = pntNode.parentNode ? (pntNode.nodeType === 11 ? 0 : (pntNode.getAttribute(tmplItmAtt) || 0)) : 0;
                    if (!(tmplItem = newTmplItems[key])) {
                        // The item is for wrapped content, and was copied from the temporary parent wrappedItem.
                        tmplItem = wrappedItems[key];
                        tmplItem = newTmplItem(tmplItem, newTmplItems[pntNode] || wrappedItems[pntNode], null, true);
                        tmplItem.key = ++itemKey;
                        newTmplItems[itemKey] = tmplItem;
                    }
                    if (cloneIndex) {
                        cloneTmplItem(key);
                    }
                }
                el.removeAttribute(tmplItmAtt);
            } else if (cloneIndex && (tmplItem = jQuery.data(el, "tmplItem"))) {
                // This was a rendered element, cloned during append or appendTo etc.
                // TmplItem stored in jQuery data has already been cloned in cloneCopyEvent. We must replace it with a fresh cloned tmplItem.
                cloneTmplItem(tmplItem.key);
                newTmplItems[tmplItem.key] = tmplItem;
                pntNode = jQuery.data(el.parentNode, "tmplItem");
                pntNode = pntNode ? pntNode.key : 0;
            }
            if (tmplItem) {
                pntItem = tmplItem;
                // Find the template item of the parent element. 
                // (Using !=, not !==, since pntItem.key is number, and pntNode may be a string)
                while (pntItem && pntItem.key != pntNode) {
                    // Add this element as a top-level node for this rendered template item, as well as for any
                    // ancestor items between this item and the item of its parent element
                    pntItem.nodes.push(el);
                    pntItem = pntItem.parent;
                }
                // Delete content built during rendering - reduce API surface area and memory use, and avoid exposing of stale data after rendering...
                delete tmplItem._ctnt;
                delete tmplItem._wrap;
                // Store template item as jQuery data on the element
                jQuery.data(el, "tmplItem", tmplItem);
            }
            function cloneTmplItem(key) {
                key = key + keySuffix;
                tmplItem = newClonedItems[key] =
                    (newClonedItems[key] || newTmplItem(tmplItem, newTmplItems[tmplItem.parent.key + keySuffix] || tmplItem.parent, null, true));
            }
        }
    }

    //---- Helper functions for template item ----

    function tiCalls(content, tmpl, data, options) {
        if (!content) {
            return stack.pop();
        }
        stack.push({ _: content, tmpl: tmpl, item: this, data: data, options: options });
    }

    function tiNest(tmpl, data, options) {
        // nested template, using {{tmpl}} tag
        return jQuery.tmpl(jQuery.template(tmpl), data, options, this);
    }

    function tiWrap(call, wrapped) {
        // nested template, using {{wrap}} tag
        var options = call.options || {};
        options.wrapped = wrapped;
        // Apply the template, which may incorporate wrapped content, 
        return jQuery.tmpl(jQuery.template(call.tmpl), call.data, options, call.item);
    }

    function tiHtml(filter, textOnly) {
        var wrapped = this._wrap;
        return jQuery.map(
            jQuery(jQuery.isArray(wrapped) ? wrapped.join("") : wrapped).filter(filter || "*"),
            function (e) {
                return textOnly ?
                    e.innerText || e.textContent :
                    e.outerHTML || outerHtml(e);
            });
    }

    function tiUpdate() {
        var coll = this.nodes;
        jQuery.tmpl(null, null, null, this).insertBefore(coll[0]);
        jQuery(coll).remove();
    }
})(jQuery);
/*
 * jQuery Templating Plugin
 * Copyright 2010, John Resig
 * Dual licensed under the MIT or GPL Version 2 licenses.
 */
(function (jQuery, undefined) {
    var oldManip = jQuery.fn.domManip, tmplItmAtt = "_tmplitem", htmlExpr = /^[^<]*(<[\w\W]+>)[^>]*$|\{\{\! /,
        newTmplItems = {}, wrappedItems = {}, appendToTmplItems, topTmplItem = { key: 0, data: {} }, itemKey = 0, cloneIndex = 0, stack = [];

    function newTmplItem(options, parentItem, fn, data) {
        // Returns a template item data structure for a new rendered instance of a template (a 'template item').
        // The content field is a hierarchical array of strings and nested items (to be
        // removed and replaced by nodes field of dom elements, once inserted in DOM).
        var newItem = {
            data: data || (parentItem ? parentItem.data : {}),
            _wrap: parentItem ? parentItem._wrap : null,
            tmpl: null,
            parent: parentItem || null,
            nodes: [],
            calls: tiCalls,
            nest: tiNest,
            wrap: tiWrap,
            html: tiHtml,
            update: tiUpdate
        };
        if (options) {
            jQuery.extend(newItem, options, { nodes: [], parent: parentItem });
        }
        if (fn) {
            // Build the hierarchical content to be used during insertion into DOM
            newItem.tmpl = fn;
            newItem._ctnt = newItem._ctnt || newItem.tmpl(jQuery, newItem);
            newItem.key = ++itemKey;
            // Keep track of new template item, until it is stored as jQuery Data on DOM element
            (stack.length ? wrappedItems : newTmplItems)[itemKey] = newItem;
        }
        return newItem;
    }

    // Override appendTo etc., in order to provide support for targeting multiple elements. (This code would disappear if integrated in jquery core).
    jQuery.each({
        appendTo: "append",
        prependTo: "prepend",
        insertBefore: "before",
        insertAfter: "after",
        replaceAll: "replaceWith"
    }, function (name, original) {
        jQuery.fn[name] = function (selector) {
            var ret = [], insert = jQuery(selector), elems, i, l, tmplItems,
                parent = this.length === 1 && this[0].parentNode;

            appendToTmplItems = newTmplItems || {};
            if (parent && parent.nodeType === 11 && parent.childNodes.length === 1 && insert.length === 1) {
                insert[original](this[0]);
                ret = this;
            } else {
                for (i = 0, l = insert.length; i < l; i++) {
                    cloneIndex = i;
                    elems = (i > 0 ? this.clone(true) : this).get();
                    jQuery.fn[original].apply(jQuery(insert[i]), elems);
                    ret = ret.concat(elems);
                }
                cloneIndex = 0;
                ret = this.pushStack(ret, name, insert.selector);
            }
            tmplItems = appendToTmplItems;
            appendToTmplItems = null;
            jQuery.tmpl.complete(tmplItems);
            return ret;
        };
    });

    jQuery.fn.extend({
        // Use first wrapped element as template markup.
        // Return wrapped set of template items, obtained by rendering template against data.
        tmpl: function (data, options, parentItem) {
            return jQuery.tmpl(this[0], data, options, parentItem);
        },

        // Find which rendered template item the first wrapped DOM element belongs to
        tmplItem: function () {
            return jQuery.tmplItem(this[0]);
        },

        // Consider the first wrapped element as a template declaration, and get the compiled template or store it as a named template.
        template: function (name) {
            return jQuery.template(name, this[0]);
        },

        domManip: function (args, table, callback, options) {
            // This appears to be a bug in the appendTo, etc. implementation
            // it should be doing .call() instead of .apply(). See #6227
            if (args[0] && args[0].nodeType) {
                var dmArgs = jQuery.makeArray(arguments), argsLength = args.length, i = 0, tmplItem;
                while (i < argsLength && !(tmplItem = jQuery.data(args[i++], "tmplItem"))) { }
                if (argsLength > 1) {
                    dmArgs[0] = [jQuery.makeArray(args)];
                }
                if (tmplItem && cloneIndex) {
                    dmArgs[2] = function (fragClone) {
                        // Handler called by oldManip when rendered template has been inserted into DOM.
                        jQuery.tmpl.afterManip(this, fragClone, callback);
                    };
                }
                oldManip.apply(this, dmArgs);
            } else {
                oldManip.apply(this, arguments);
            }
            cloneIndex = 0;
            if (!appendToTmplItems) {
                jQuery.tmpl.complete(newTmplItems);
            }
            return this;
        }
    });

    jQuery.extend({
        // Return wrapped set of template items, obtained by rendering template against data.
        tmpl: function (tmpl, data, options, parentItem) {
            var ret, topLevel = !parentItem;
            if (topLevel) {
                // This is a top-level tmpl call (not from a nested template using {{tmpl}})
                parentItem = topTmplItem;
                tmpl = jQuery.template[tmpl] || jQuery.template(null, tmpl);
                wrappedItems = {}; // Any wrapped items will be rebuilt, since this is top level
            } else if (!tmpl) {
                // The template item is already associated with DOM - this is a refresh.
                // Re-evaluate rendered template for the parentItem
                tmpl = parentItem.tmpl;
                newTmplItems[parentItem.key] = parentItem;
                parentItem.nodes = [];
                if (parentItem.wrapped) {
                    updateWrapped(parentItem, parentItem.wrapped);
                }
                // Rebuild, without creating a new template item
                return jQuery(build(parentItem, null, parentItem.tmpl(jQuery, parentItem)));
            }
            if (!tmpl) {
                return []; // Could throw...
            }
            if (typeof data === "function") {
                data = data.call(parentItem || {});
            }
            if (options && options.wrapped) {
                updateWrapped(options, options.wrapped);
            }
            ret = jQuery.isArray(data) ?
                jQuery.map(data, function (dataItem) {
                    return dataItem ? newTmplItem(options, parentItem, tmpl, dataItem) : null;
                }) :
                [newTmplItem(options, parentItem, tmpl, data)];
            return topLevel ? jQuery(build(parentItem, null, ret)) : ret;
        },

        // Return rendered template item for an element.
        tmplItem: function (elem) {
            var tmplItem;
            if (elem instanceof jQuery) {
                elem = elem[0];
            }
            while (elem && elem.nodeType === 1 && !(tmplItem = jQuery.data(elem, "tmplItem")) && (elem = elem.parentNode)) { }
            return tmplItem || topTmplItem;
        },

        // Set:
        // Use $.template( name, tmpl ) to cache a named template,
        // where tmpl is a template string, a script element or a jQuery instance wrapping a script element, etc.
        // Use $( "selector" ).template( name ) to provide access by name to a script block template declaration.

        // Get:
        // Use $.template( name ) to access a cached template.
        // Also $( selectorToScriptBlock ).template(), or $.template( null, templateString )
        // will return the compiled template, without adding a name reference.
        // If templateString includes at least one HTML tag, $.template( templateString ) is equivalent
        // to $.template( null, templateString )
        template: function (name, tmpl) {
            if (tmpl) {
                // Compile template and associate with name
                if (typeof tmpl === "string") {
                    // This is an HTML string being passed directly in.
                    tmpl = buildTmplFn(tmpl)
                } else if (tmpl instanceof jQuery) {
                    tmpl = tmpl[0] || {};
                }
                if (tmpl.nodeType) {
                    // If this is a template block, use cached copy, or generate tmpl function and cache.
                    tmpl = jQuery.data(tmpl, "tmpl") || jQuery.data(tmpl, "tmpl", buildTmplFn(tmpl.innerHTML));
                }
                return typeof name === "string" ? (jQuery.template[name] = tmpl) : tmpl;
            }
            // Return named compiled template
            return name ? (typeof name !== "string" ? jQuery.template(null, name) :
                (jQuery.template[name] ||
                    // If not in map, treat as a selector. (If integrated with core, use quickExpr.exec) 
                    jQuery.template(null, htmlExpr.test(name) ? name : jQuery(name)))) : null;
        },

        encode: function (text) {
            // Do HTML encoding replacing < > & and ' and " by corresponding entities.
            return ("" + text).split("<").join("&lt;").split(">").join("&gt;").split('"').join("&#34;").split("'").join("&#39;");
        }
    });

    jQuery.extend(jQuery.tmpl, {
        tag: {
            "tmpl": {
                _default: { $2: "null" },
                open: "if($notnull_1){_=_.concat($item.nest($1,$2));}"
                // tmpl target parameter can be of type function, so use $1, not $1a (so not auto detection of functions)
                // This means that {{tmpl foo}} treats foo as a template (which IS a function). 
                // Explicit parens can be used if foo is a function that returns a template: {{tmpl foo()}}.
            },
            "wrap": {
                _default: { $2: "null" },
                open: "$item.calls(_,$1,$2);_=[];",
                close: "call=$item.calls();_=call._.concat($item.wrap(call,_));"
            },
            "each": {
                _default: { $2: "$index, $value" },
                open: "if($notnull_1){$.each($1a,function($2){with(this){",
                close: "}});}"
            },
            "if": {
                open: "if(($notnull_1) && $1a){",
                close: "}"
            },
            "else": {
                _default: { $1: "true" },
                open: "}else if(($notnull_1) && $1a){"
            },
            "html": {
                // Unecoded expression evaluation. 
                open: "if($notnull_1){_.push($1a);}"
            },
            "=": {
                // Encoded expression evaluation. Abbreviated form is ${}.
                _default: { $1: "$data" },
                open: "if($notnull_1){_.push($.encode($1a));}"
            },
            "!": {
                // Comment tag. Skipped by parser
                open: ""
            }
        },

        // This stub can be overridden, e.g. in jquery.tmplPlus for providing rendered events
        complete: function (items) {
            newTmplItems = {};
        },

        // Call this from code which overrides domManip, or equivalent
        // Manage cloning/storing template items etc.
        afterManip: function afterManip(elem, fragClone, callback) {
            // Provides cloned fragment ready for fixup prior to and after insertion into DOM
            var content = fragClone.nodeType === 11 ?
                jQuery.makeArray(fragClone.childNodes) :
                fragClone.nodeType === 1 ? [fragClone] : [];

            // Return fragment to original caller (e.g. append) for DOM insertion
            callback.call(elem, fragClone);

            // Fragment has been inserted:- Add inserted nodes to tmplItem data structure. Replace inserted element annotations by jQuery.data.
            storeTmplItems(content);
            cloneIndex++;
        }
    });

    //========================== Private helper functions, used by code above ==========================

    function build(tmplItem, nested, content) {
        // Convert hierarchical content into flat string array 
        // and finally return array of fragments ready for DOM insertion
        var frag, ret = content ? jQuery.map(content, function (item) {
            return (typeof item === "string") ?
                // Insert template item annotations, to be converted to jQuery.data( "tmplItem" ) when elems are inserted into DOM.
                (tmplItem.key ? item.replace(/(<\w+)(?=[\s>])(?![^>]*_tmplitem)([^>]*)/g, "$1 " + tmplItmAtt + "=\"" + tmplItem.key + "\" $2") : item) :
                // This is a child template item. Build nested template.
                build(item, tmplItem, item._ctnt);
        }) :
            // If content is not defined, insert tmplItem directly. Not a template item. May be a string, or a string array, e.g. from {{html $item.html()}}. 
            tmplItem;
        if (nested) {
            return ret;
        }

        // top-level template
        ret = ret.join("");

        // Support templates which have initial or final text nodes, or consist only of text
        // Also support HTML entities within the HTML markup.
        ret.replace(/^\s*([^<\s][^<]*)?(<[\w\W]+>)([^>]*[^>\s])?\s*$/, function (all, before, middle, after) {
            frag = jQuery(middle).get();

            storeTmplItems(frag);
            if (before) {
                frag = unencode(before).concat(frag);
            }
            if (after) {
                frag = frag.concat(unencode(after));
            }
        });
        return frag ? frag : unencode(ret);
    }

    function unencode(text) {
        // Use createElement, since createTextNode will not render HTML entities correctly
        var el = document.createElement("div");
        el.innerHTML = text;
        return jQuery.makeArray(el.childNodes);
    }

    // Generate a reusable function that will serve to render a template against data
    function buildTmplFn(markup) {
        return new Function("jQuery", "$item",
            "var $=jQuery,call,_=[],$data=$item.data;" +

            // Introduce the data as local variables using with(){}
            "with($data){_.push('" +

            // Convert the template into pure JavaScript
            jQuery.trim(markup)
                .replace(/([\\'])/g, "\\$1")
                .replace(/[\r\t\n]/g, " ")
                .replace(/\$\{([^\}]*)\}/g, "{{= $1}}")
                .replace(/\{\{(\/?)(\w+|.)(?:\(((?:[^\}]|\}(?!\}))*?)?\))?(?:\s+(.*?)?)?(\(((?:[^\}]|\}(?!\}))*?)\))?\s*\}\}/g,
                function (all, slash, type, fnargs, target, parens, args) {
                    var tag = jQuery.tmpl.tag[type], def, expr, exprAutoFnDetect;
                    if (!tag) {
                        throw "Template command not found: " + type;
                    }
                    def = tag._default || [];
                    if (parens && !/\w$/.test(target)) {
                        target += parens;
                        parens = "";
                    }
                    if (target) {
                        target = unescape(target);
                        args = args ? ("," + unescape(args) + ")") : (parens ? ")" : "");
                        // Support for target being things like a.toLowerCase();
                        // In that case don't call with template item as 'this' pointer. Just evaluate...
                        expr = parens ? (target.indexOf(".") > -1 ? target + parens : ("(" + target + ").call($item" + args)) : target;
                        exprAutoFnDetect = parens ? expr : "(typeof(" + target + ")==='function'?(" + target + ").call($item):(" + target + "))";
                    } else {
                        exprAutoFnDetect = expr = def.$1 || "null";
                    }
                    fnargs = unescape(fnargs);
                    return "');" +
                        tag[slash ? "close" : "open"]
                            .split("$notnull_1").join(target ? "typeof(" + target + ")!=='undefined' && (" + target + ")!=null" : "true")
                            .split("$1a").join(exprAutoFnDetect)
                            .split("$1").join(expr)
                            .split("$2").join(fnargs ?
                                fnargs.replace(/\s*([^\(]+)\s*(\((.*?)\))?/g, function (all, name, parens, params) {
                                    params = params ? ("," + params + ")") : (parens ? ")" : "");
                                    return params ? ("(" + name + ").call($item" + params) : all;
                                })
                                : (def.$2 || "")
                            ) +
                        "_.push('";
                }) +
            "');}return _;"
        );
    }
    function updateWrapped(options, wrapped) {
        // Build the wrapped content. 
        options._wrap = build(options, true,
            // Suport imperative scenario in which options.wrapped can be set to a selector or an HTML string.
            jQuery.isArray(wrapped) ? wrapped : [htmlExpr.test(wrapped) ? wrapped : jQuery(wrapped).html()]
        ).join("");
    }

    function unescape(args) {
        return args ? args.replace(/\\'/g, "'").replace(/\\\\/g, "\\") : null;
    }
    function outerHtml(elem) {
        var div = document.createElement("div");
        div.appendChild(elem.cloneNode(true));
        return div.innerHTML;
    }

    // Store template items in jQuery.data(), ensuring a unique tmplItem data data structure for each rendered template instance.
    function storeTmplItems(content) {
        var keySuffix = "_" + cloneIndex, elem, elems, newClonedItems = {}, i, l, m;
        for (i = 0, l = content.length; i < l; i++) {
            if ((elem = content[i]).nodeType !== 1) {
                continue;
            }
            elems = elem.getElementsByTagName("*");
            for (m = elems.length - 1; m >= 0; m--) {
                processItemKey(elems[m]);
            }
            processItemKey(elem);
        }
        function processItemKey(el) {
            var pntKey, pntNode = el, pntItem, tmplItem, key;
            // Ensure that each rendered template inserted into the DOM has its own template item,
            if ((key = el.getAttribute(tmplItmAtt))) {
                while (pntNode.parentNode && (pntNode = pntNode.parentNode).nodeType === 1 && !(pntKey = pntNode.getAttribute(tmplItmAtt))) { }
                if (pntKey !== key) {
                    // The next ancestor with a _tmplitem expando is on a different key than this one.
                    // So this is a top-level element within this template item
                    // Set pntNode to the key of the parentNode, or to 0 if pntNode.parentNode is null, or pntNode is a fragment.
                    pntNode = pntNode.parentNode ? (pntNode.nodeType === 11 ? 0 : (pntNode.getAttribute(tmplItmAtt) || 0)) : 0;
                    if (!(tmplItem = newTmplItems[key])) {
                        // The item is for wrapped content, and was copied from the temporary parent wrappedItem.
                        tmplItem = wrappedItems[key];
                        tmplItem = newTmplItem(tmplItem, newTmplItems[pntNode] || wrappedItems[pntNode], null, true);
                        tmplItem.key = ++itemKey;
                        newTmplItems[itemKey] = tmplItem;
                    }
                    if (cloneIndex) {
                        cloneTmplItem(key);
                    }
                }
                el.removeAttribute(tmplItmAtt);
            } else if (cloneIndex && (tmplItem = jQuery.data(el, "tmplItem"))) {
                // This was a rendered element, cloned during append or appendTo etc.
                // TmplItem stored in jQuery data has already been cloned in cloneCopyEvent. We must replace it with a fresh cloned tmplItem.
                cloneTmplItem(tmplItem.key);
                newTmplItems[tmplItem.key] = tmplItem;
                pntNode = jQuery.data(el.parentNode, "tmplItem");
                pntNode = pntNode ? pntNode.key : 0;
            }
            if (tmplItem) {
                pntItem = tmplItem;
                // Find the template item of the parent element. 
                // (Using !=, not !==, since pntItem.key is number, and pntNode may be a string)
                while (pntItem && pntItem.key != pntNode) {
                    // Add this element as a top-level node for this rendered template item, as well as for any
                    // ancestor items between this item and the item of its parent element
                    pntItem.nodes.push(el);
                    pntItem = pntItem.parent;
                }
                // Delete content built during rendering - reduce API surface area and memory use, and avoid exposing of stale data after rendering...
                delete tmplItem._ctnt;
                delete tmplItem._wrap;
                // Store template item as jQuery data on the element
                jQuery.data(el, "tmplItem", tmplItem);
            }
            function cloneTmplItem(key) {
                key = key + keySuffix;
                tmplItem = newClonedItems[key] =
                    (newClonedItems[key] || newTmplItem(tmplItem, newTmplItems[tmplItem.parent.key + keySuffix] || tmplItem.parent, null, true));
            }
        }
    }

    //---- Helper functions for template item ----

    function tiCalls(content, tmpl, data, options) {
        if (!content) {
            return stack.pop();
        }
        stack.push({ _: content, tmpl: tmpl, item: this, data: data, options: options });
    }

    function tiNest(tmpl, data, options) {
        // nested template, using {{tmpl}} tag
        return jQuery.tmpl(jQuery.template(tmpl), data, options, this);
    }

    function tiWrap(call, wrapped) {
        // nested template, using {{wrap}} tag
        var options = call.options || {};
        options.wrapped = wrapped;
        // Apply the template, which may incorporate wrapped content, 
        return jQuery.tmpl(jQuery.template(call.tmpl), call.data, options, call.item);
    }

    function tiHtml(filter, textOnly) {
        var wrapped = this._wrap;
        return jQuery.map(
            jQuery(jQuery.isArray(wrapped) ? wrapped.join("") : wrapped).filter(filter || "*"),
            function (e) {
                return textOnly ?
                    e.innerText || e.textContent :
                    e.outerHTML || outerHtml(e);
            });
    }

    function tiUpdate() {
        var coll = this.nodes;
        jQuery.tmpl(null, null, null, this).insertBefore(coll[0]);
        jQuery(coll).remove();
    }
})(jQuery);
$(function () {
    var BootstrapDiv = function (el, options) {
        var $this = this;
        $this.options = options;
        $this.$el = $(el);
        $this.$el_ = this.$el.clone();
        $this.methods = {
            calculateObjectValue: function (self, name, args, defaultValue) {
                var func = name;

                if (typeof name === 'string') {
                    // support obj.func1.func2
                    var names = name.split('.');

                    if (names.length > 1) {
                        func = window;
                        $.each(names, function (i, f) {
                            func = func[f];
                        });
                    } else {
                        func = window[name];
                    }
                }
                if (typeof func === 'object') {
                    return func;
                }
                if (typeof func === 'function') {
                    return func.apply(self, args || []);
                }
                if (!func && typeof name === 'string' && sprintf.apply($this, [name].concat(args))) {
                    return sprintf.apply($this, [name].concat(args));
                }
                return defaultValue;
            },
            refresh: function () {
                $this.options.pageNumber = 1;
                $this.methods.init();
            },
            init: function () {
                var that = $this,
                    data = {},
                    params = {
                        sortName: $this.options.sortName,
                        sortOrder: $this.options.sortOrder
                    },
                    request;

                if (!$this.options.url) {
                    return;
                }

                params = {
                    sort: params.sortName,
                    order: params.sortOrder
                };

                if ($this.options.pagination) {
                    params.offset = ($this.options.pageNumber - 1) * $this.options.pageSize;
                    params.limit = $this.options.pageSize;
                }


                data = $this.methods.calculateObjectValue($this.options, $this.options.queryParams, [params], data);

                //$.extend(data, query || {});

                // false to stop request
                if (data === false) {
                    return;
                }

                request = $.extend({}, $this.methods.calculateObjectValue(null, $this.options.ajaxOptions), {
                    type: $this.options.method,
                    url: $this.options.url,
                    data: $this.options.contentType === 'application/json' && $this.options.method === 'post' ?
                        JSON.stringify(data) : data,
                    cache: false,
                    contentType: $this.options.contentType,
                    dataType: $this.options.dataType,
                    success: function (res) {
                        if (typeof res.StatusCode !== "undefined") {
                            isSuccess(res);
                            return;
                        }
                        res = that.methods.calculateObjectValue(that.options, that.options.responseHandler, [res], res);
                        var totalPages = res.total / that.options.pageSize;
                        if (res.total % that.options.pageSize > 0 && res.total > that.options.pageSize) {
                            totalPages++;
                        }
                        $(that.$el).html("");
                        if (res.total === 0 && that.options.noFoundText!="") {
                            var div = $("<div class='text-center'><div class='table-bordered'><div class='panel-body'></div></div></div>");
                            div.find(".panel-body").text(that.options.noFoundText);
                            div.appendTo(that.$el);
                        }
                        var htmlTemplate = that.$el_.find("#bt-div-template").html();
                        var currentOffset = ((that.options.pageNumber - 1) * that.options.pageSize);
                        $(res.rows).each(function (i, item) {
                            item["_RowNumber_"] = currentOffset + i + 1;
                            var dataItem = $.tmpl(htmlTemplate, item);
                            $(dataItem).appendTo(that.$el);
                        });
                        $("<div class='clearfix'/>").appendTo(that.$el);
                        $(that.$el).bootpag({
                            total: Math.floor(totalPages),
                            page: that.options.pageNumber,
                            maxVisible: 5
                        }).on("page", function (event, num) {
                            that.options.pageNumber = num;
                            that.methods.init();
                        });
                        $("<div class='clearfix'/>").appendTo(that.$el);
                        if (typeof that.options.loadSuccess !== "undefined" && that.options.loadSuccess != null) {
                            return window[that.options.loadSuccess](that, res);
                        }
                    },
                    error: function (res) {
                        Failed(res);
                    }
                });
                $(that.$el).html("");
                var div = $("<div class='text-center'><div class='table-bordered'><div class='panel-body'></div></div></div>");
                div.find(".panel-body").text(that.options.loadingText);
                div.appendTo(that.$el);
                $.ajax(request);
            }
        };
        $this.methods.init();
    };

    BootstrapDiv.DEFAULTS = {
        sortName: undefined,
        sortOrder: "asc",
        data: [],
        method: "post",
        url: undefined,
        ajax: undefined,
        contentType: "application/json",
        dataType: "json",
        queryParams: function (params) {
            return params;
        },
        pageNumber: 1,
        pageSize: 10,
        noFoundText: "No data found.",
        loadingText: "Loading please wait...",
        loadSuccess: null,
        pagination: true,
        ajaxOptions: null
    };

    $.fn.bootstrapDiv = function (option) {
        var value, args = Array.prototype.slice.call(arguments, 1);

        this.each(function () {
            var $this = $(this),
                data = $this.data('bootstrap.div'),
                options = $.extend({}, BootstrapDiv.DEFAULTS, $this.data(),
                    typeof option === 'object' && option);

            if (typeof option === 'string') {
                if ($.inArray(option, ['refresh']) < 0) {
                    throw new Error("Unknown method: " + option);
                }

                if (!data) {
                    return;
                }

                value = data.methods[option].apply(data, args);

                if (option === 'destroy') {
                    $this.removeData('bootstrap.div');
                }
            }

            if (!data) {
                $this.data('bootstrap.div', (data = new BootstrapDiv(this, options)));
            }
        });

        return typeof value === 'undefined' ? this : value;
    };

    $.fn.bootstrapDivRefresh = function () {
        $(this).bootstrapDiv("refresh");
    };

    BindBtDiv("body");

});

function BindBtDiv(selector) {
    $(selector + ' [data-toggle="btdiv"]').each(function () {
        var $btdiv = $(this);
        $btdiv.bootstrapDiv({
            pageNumber: 1,
            queryParams: function (params) {
                var fn = $btdiv.attr("data-queryparams");
                if (typeof fn !== "undefined" && fn !== "") {
                    return window[fn]($btdiv, params);
                } else {
                    return params;
                }
            },
            ajaxOptions: { headers: { "Authorization": AuthHeader  } }
        });
    });
}
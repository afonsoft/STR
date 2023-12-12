this.Element &&
    (function (t) {
        t.matches =
            t.matches ||
            t.matchesSelector ||
            t.webkitMatchesSelector ||
            t.msMatchesSelector ||
            function (t) {
                for (var e = (this.parentNode || this.document).querySelectorAll(t), n = -1; e[++n] && e[n] != this; );
                return !!e[n];
            };
    })(Element.prototype),
    this.Element &&
        (function (t) {
            t.closest =
                t.closest ||
                function (t) {
                    for (var e = this; e.matches && !e.matches(t); ) e = e.parentNode;
                    return e.matches ? e : null;
                };
        })(Element.prototype),
    this.Element &&
        (function (t) {
            t.matches =
                t.matches ||
                t.matchesSelector ||
                t.webkitMatchesSelector ||
                t.msMatchesSelector ||
                function (t) {
                    for (var e = (this.parentNode || this.document).querySelectorAll(t), n = -1; e[++n] && e[n] != this; );
                    return !!e[n];
                };
        })(Element.prototype),
    (function () {
        for (var t = 0, e = ["webkit", "moz"], n = 0; n < e.length && !window.requestAnimationFrame; ++n)
            (window.requestAnimationFrame = window[e[n] + "RequestAnimationFrame"]), (window.cancelAnimationFrame = window[e[n] + "CancelAnimationFrame"] || window[e[n] + "CancelRequestAnimationFrame"]);
        window.requestAnimationFrame ||
            (window.requestAnimationFrame = function (e) {
                var n = new Date().getTime(),
                    o = Math.max(0, 16 - (n - t)),
                    i = window.setTimeout(function () {
                        e(n + o);
                    }, o);
                return (t = n + o), i;
            }),
            window.cancelAnimationFrame ||
                (window.cancelAnimationFrame = function (t) {
                    clearTimeout(t);
                });
    })(),
    [Element.prototype, Document.prototype, DocumentFragment.prototype].forEach(function (t) {
        t.hasOwnProperty("prepend") ||
            Object.defineProperty(t, "prepend", {
                configurable: !0,
                enumerable: !0,
                writable: !0,
                value: function () {
                    var t = Array.prototype.slice.call(arguments),
                        e = document.createDocumentFragment();
                    t.forEach(function (t) {
                        var n = t instanceof Node;
                        e.appendChild(n ? t : document.createTextNode(String(t)));
                    }),
                        this.insertBefore(e, this.firstChild);
                },
            });
    }),
    (window.mUtilElementDataStore = {}),
    (window.mUtilElementDataStoreID = 0),
    (window.mUtilDelegatedEventHandlers = {});
var mUtil = (function () {
    var t = [],
        e = { sm: 544, md: 768, lg: 1024, xl: 1200 },
        n = function () {
            var e = !1;
            window.addEventListener("resize", function () {
                clearTimeout(e),
                    (e = setTimeout(function () {
                        !(function () {
                            for (var e = 0; e < t.length; e++) t[e].call();
                        })();
                    }, 250));
            });
        };
    return {
        init: function (t) {
            t && t.breakpoints && (e = t.breakpoints), n();
        },
        addResizeHandler: function (e) {
            t.push(e);
        },
        removeResizeHandler: function (e) {
            for (var n = 0; n < t.length; n++) e === t[n] && delete t[n];
        },
        runResizeHandlers: function () {
            _runResizeHandlers();
        },
        resize: function () {
            if ("function" == typeof Event) window.dispatchEvent(new Event("resize"));
            else {
                var t = window.document.createEvent("UIEvents");
                t.initUIEvent("resize", !0, !1, window, 0), window.dispatchEvent(t);
            }
        },
        isMobileDevice: function () {
            return this.getViewPort().width < this.getBreakpoint("lg");
        },
        isDesktopDevice: function () {
            return !mUtil.isMobileDevice();
        },
        getViewPort: function () {
            var t = window,
                e = "inner";
            return "innerWidth" in window || ((e = "client"), (t = document.documentElement || document.body)), { width: t[e + "Width"], height: t[e + "Height"] };
        },
        isInResponsiveRange: function (t) {
            var e = this.getViewPort().width;
            return (
                "general" == t ||
                ("desktop" == t && e >= this.getBreakpoint("lg") + 1) ||
                ("tablet" == t && e >= this.getBreakpoint("md") + 1 && e < this.getBreakpoint("lg")) ||
                ("mobile" == t && e <= this.getBreakpoint("md")) ||
                ("desktop-and-tablet" == t && e >= this.getBreakpoint("md") + 1) ||
                ("tablet-and-mobile" == t && e <= this.getBreakpoint("lg")) ||
                ("minimal-desktop-and-below" == t && e <= this.getBreakpoint("xl"))
            );
        },
        getUniqueID: function (t) {
            return t + Math.floor(Math.random() * new Date().getTime());
        },
        getBreakpoint: function (t) {
            return e[t];
        },
        isset: function (t, e) {
            var n;
            if (-1 !== (e = e || "").indexOf("[")) throw new Error("Unsupported object path notation.");
            e = e.split(".");
            do {
                if (void 0 === t) return !1;
                if (((n = e.shift()), !t.hasOwnProperty(n))) return !1;
                t = t[n];
            } while (e.length);
            return !0;
        },
        getHighestZindex: function (t) {
            for (var e, n, o = mUtil.get(t); o && o !== document; ) {
                if (("absolute" === (e = mUtil.css(o, "position")) || "relative" === e || "fixed" === e) && ((n = parseInt(mUtil.css(o, "z-index"))), !isNaN(n) && 0 !== n)) return n;
                o = o.parentNode;
            }
            return null;
        },
        hasFixedPositionedParent: function (t) {
            for (; t && t !== document; ) {
                if (((position = mUtil.css(t, "position")), "fixed" === position)) return !0;
                t = t.parentNode;
            }
            return !1;
        },
        sleep: function (t) {
            for (var e = new Date().getTime(), n = 0; n < 1e7 && !(new Date().getTime() - e > t); n++);
        },
        getRandomInt: function (t, e) {
            return Math.floor(Math.random() * (e - t + 1)) + t;
        },
        isAngularVersion: function () {
            return void 0 !== window.Zone;
        },
        deepExtend: function (t) {
            t = t || {};
            for (var e = 1; e < arguments.length; e++) {
                var n = arguments[e];
                if (n) for (var o in n) n.hasOwnProperty(o) && ("object" == typeof n[o] ? (t[o] = mUtil.deepExtend(t[o], n[o])) : (t[o] = n[o]));
            }
            return t;
        },
        extend: function (t) {
            t = t || {};
            for (var e = 1; e < arguments.length; e++) if (arguments[e]) for (var n in arguments[e]) arguments[e].hasOwnProperty(n) && (t[n] = arguments[e][n]);
            return t;
        },
        get: function (t) {
            var e;
            return t === document ? document : t && 1 === t.nodeType ? t : (e = document.getElementById(t)) ? e : (e = document.getElementsByTagName(t)) ? e[0] : (e = document.getElementsByClassName(t)) ? e[0] : null;
        },
        getByClass: function (t) {
            var e;
            return (e = document.getElementsByClassName(t)) ? e[0] : null;
        },
        hasClasses: function (t, e) {
            if (t) {
                for (var n = e.split(" "), o = 0; o < n.length; o++) if (0 == mUtil.hasClass(t, mUtil.trim(n[o]))) return !1;
                return !0;
            }
        },
        hasClass: function (t, e) {
            if (t) return t.classList ? t.classList.contains(e) : new RegExp("\\b" + e + "\\b").test(t.className);
        },
        addClass: function (t, e) {
            if (t && void 0 !== e) {
                var n = e.split(" ");
                if (t.classList) for (var o = 0; o < n.length; o++) n[o] && n[o].length > 0 && t.classList.add(mUtil.trim(n[o]));
                else if (!mUtil.hasClass(t, e)) for (o = 0; o < n.length; o++) t.className += " " + mUtil.trim(n[o]);
            }
        },
        removeClass: function (t, e) {
            if (t && void 0 !== e) {
                var n = e.split(" ");
                if (t.classList) for (var o = 0; o < n.length; o++) t.classList.remove(mUtil.trim(n[o]));
                else if (mUtil.hasClass(t, e)) for (o = 0; o < n.length; o++) t.className = t.className.replace(new RegExp("\\b" + mUtil.trim(n[o]) + "\\b", "g"), "");
            }
        },
        triggerCustomEvent: function (t, e, n) {
            if (window.CustomEvent) var o = new CustomEvent(e, { detail: n });
            else (o = document.createEvent("CustomEvent")).initCustomEvent(e, !0, !0, n);
            t.dispatchEvent(o);
        },
        trim: function (t) {
            return t.trim();
        },
        eventTriggered: function (t) {
            return !!t.currentTarget.dataset.triggered || ((t.currentTarget.dataset.triggered = !0), !1);
        },
        remove: function (t) {
            t && t.parentNode && t.parentNode.removeChild(t);
        },
        find: function (t, e) {
            if ((t = mUtil.get(t))) return t.querySelector(e);
        },
        findAll: function (t, e) {
            if ((t = mUtil.get(t))) return t.querySelectorAll(e);
        },
        insertAfter: function (t, e) {
            return e.parentNode.insertBefore(t, e.nextSibling);
        },
        parents: function (t, e) {
            function n(t, e) {
                for (var n = 0, o = t.length; n < o; n++) if (t[n] == e) return !0;
                return !1;
            }
            return (function (t, e) {
                for (var o = document.querySelectorAll(e), i = t.parentNode; i && !n(o, i); ) i = i.parentNode;
                return i;
            })(t, e);
        },
        children: function (t, e, n) {
            if (t && t.childNodes) {
                for (var o = [], i = 0, l = t.childNodes.length; i < l; ++i) 1 == t.childNodes[i].nodeType && mUtil.matches(t.childNodes[i], e, n) && o.push(t.childNodes[i]);
                return o;
            }
        },
        child: function (t, e, n) {
            var o = mUtil.children(t, e, n);
            return o ? o[0] : null;
        },
        matches: function (t, e, n) {
            var o = Element.prototype,
                i =
                    o.matches ||
                    o.webkitMatchesSelector ||
                    o.mozMatchesSelector ||
                    o.msMatchesSelector ||
                    function (t) {
                        return -1 !== [].indexOf.call(document.querySelectorAll(t), this);
                    };
            return !(!t || !t.tagName) && i.call(t, e);
        },
        data: function (t) {
            return (
                (t = mUtil.get(t)),
                {
                    set: function (e, n) {
                        void 0 === t.customDataTag && (mUtilElementDataStoreID++, (t.customDataTag = mUtilElementDataStoreID)),
                            void 0 === mUtilElementDataStore[t.customDataTag] && (mUtilElementDataStore[t.customDataTag] = {}),
                            (mUtilElementDataStore[t.customDataTag][e] = n);
                    },
                    get: function (e) {
                        return this.has(e) ? mUtilElementDataStore[t.customDataTag][e] : null;
                    },
                    has: function (e) {
                        return !(!mUtilElementDataStore[t.customDataTag] || !mUtilElementDataStore[t.customDataTag][e]);
                    },
                    remove: function (e) {
                        this.has(e) && delete mUtilElementDataStore[t.customDataTag][e];
                    },
                }
            );
        },
        outerWidth: function (t, e) {
            if (!0 === e) {
                var n = parseFloat(t.offsetWidth);
                return (n += parseFloat(mUtil.css(t, "margin-left")) + parseFloat(mUtil.css(t, "margin-right"))), parseFloat(n);
            }
            return (n = parseFloat(t.offsetWidth));
        },
        offset: function (t) {
            var e, n;
            if ((t = mUtil.get(t))) return t.getClientRects().length ? ((e = t.getBoundingClientRect()), (n = t.ownerDocument.defaultView), { top: e.top + n.pageYOffset, left: e.left + n.pageXOffset }) : { top: 0, left: 0 };
        },
        height: function (t) {
            return mUtil.css(t, "height");
        },
        visible: function (t) {
            return !(0 === t.offsetWidth && 0 === t.offsetHeight);
        },
        attr: function (t, e, n) {
            if (null != (t = mUtil.get(t))) return void 0 === n ? t.getAttribute(e) : void t.setAttribute(e, n);
        },
        hasAttr: function (t, e) {
            if (null != (t = mUtil.get(t))) return !!t.getAttribute(e);
        },
        removeAttr: function (t, e) {
            null != (t = mUtil.get(t)) && t.removeAttribute(e);
        },
        animate: function (t, e, n, o, i, l) {
            var r = {};
            if (
                ((r.linear = function (t, e, n, o) {
                    return (n * t) / o + e;
                }),
                (i = r.linear),
                "number" == typeof t && "number" == typeof e && "number" == typeof n && "function" == typeof o)
            ) {
                "function" != typeof l && (l = function () {});
                var s =
                        window.requestAnimationFrame ||
                        function (t) {
                            window.setTimeout(t, 20);
                        },
                    a = e - t;
                o(t);
                var m = window.performance && window.performance.now ? window.performance.now() : +new Date();
                s(function r(d) {
                    var u = (d || +new Date()) - m;
                    u >= 0 && o(i(u, t, a, n)), u >= 0 && u >= n ? (o(e), l()) : s(r);
                });
            }
        },
        actualCss: function (t, e, n) {
            var o;
            if (t instanceof HTMLElement != !1)
                return t.getAttribute("m-hidden-" + e) && !1 !== n
                    ? parseFloat(t.getAttribute("m-hidden-" + e))
                    : ((t.style.cssText = "position: absolute; visibility: hidden; display: block;"),
                      "width" == e ? (o = t.offsetWidth) : "height" == e && (o = t.offsetHeight),
                      (t.style.cssText = ""),
                      t.setAttribute("m-hidden-" + e, o),
                      parseFloat(o));
        },
        actualHeight: function (t, e) {
            return mUtil.actualCss(t, "height", e);
        },
        actualWidth: function (t, e) {
            return mUtil.actualCss(t, "width", e);
        },
        getScroll: function (t, e) {
            return (e = "scroll" + e), t == window || t == document ? self["scrollTop" == e ? "pageYOffset" : "pageXOffset"] || (browserSupportsBoxModel && document.documentElement[e]) || document.body[e] : t[e];
        },
        css: function (t, e, n) {
            if ((t = mUtil.get(t)))
                if (void 0 !== n) t.style[e] = n;
                else {
                    var o = (t.ownerDocument || document).defaultView;
                    if (o && o.getComputedStyle) return (e = e.replace(/([A-Z])/g, "-$1").toLowerCase()), o.getComputedStyle(t, null).getPropertyValue(e);
                    if (t.currentStyle)
                        return (
                            (e = e.replace(/\-(\w)/g, function (t, e) {
                                return e.toUpperCase();
                            })),
                            (n = t.currentStyle[e]),
                            /^\d+(em|pt|%|ex)?$/i.test(n)
                                ? (function (e) {
                                      var n = t.style.left,
                                          o = t.runtimeStyle.left;
                                      return (t.runtimeStyle.left = t.currentStyle.left), (t.style.left = e || 0), (e = t.style.pixelLeft + "px"), (t.style.left = n), (t.runtimeStyle.left = o), e;
                                  })(n)
                                : n
                        );
                }
        },
        slide: function (t, e, n, o, i) {
            if (!(!t || ("up" == e && !1 === mUtil.visible(t)) || ("down" == e && !0 === mUtil.visible(t)))) {
                n = n || 600;
                var l = mUtil.actualHeight(t),
                    r = !1,
                    s = !1;
                mUtil.css(t, "padding-top") && !0 !== mUtil.data(t).has("slide-padding-top") && mUtil.data(t).set("slide-padding-top", mUtil.css(t, "padding-top")),
                    mUtil.css(t, "padding-bottom") && !0 !== mUtil.data(t).has("slide-padding-bottom") && mUtil.data(t).set("slide-padding-bottom", mUtil.css(t, "padding-bottom")),
                    mUtil.data(t).has("slide-padding-top") && (r = parseInt(mUtil.data(t).get("slide-padding-top"))),
                    mUtil.data(t).has("slide-padding-bottom") && (s = parseInt(mUtil.data(t).get("slide-padding-bottom"))),
                    "up" == e
                        ? ((t.style.cssText = "display: block; overflow: hidden;"),
                          r &&
                              mUtil.animate(
                                  0,
                                  r,
                                  n,
                                  function (e) {
                                      t.style.paddingTop = r - e + "px";
                                  },
                                  "linear"
                              ),
                          s &&
                              mUtil.animate(
                                  0,
                                  s,
                                  n,
                                  function (e) {
                                      t.style.paddingBottom = s - e + "px";
                                  },
                                  "linear"
                              ),
                          mUtil.animate(
                              0,
                              l,
                              n,
                              function (e) {
                                  t.style.height = l - e + "px";
                              },
                              "linear",
                              function () {
                                  o(), (t.style.height = ""), (t.style.display = "none");
                              }
                          ))
                        : "down" == e &&
                          ((t.style.cssText = "display: block; overflow: hidden;"),
                          r &&
                              mUtil.animate(
                                  0,
                                  r,
                                  n,
                                  function (e) {
                                      t.style.paddingTop = e + "px";
                                  },
                                  "linear",
                                  function () {
                                      t.style.paddingTop = "";
                                  }
                              ),
                          s &&
                              mUtil.animate(
                                  0,
                                  s,
                                  n,
                                  function (e) {
                                      t.style.paddingBottom = e + "px";
                                  },
                                  "linear",
                                  function () {
                                      t.style.paddingBottom = "";
                                  }
                              ),
                          mUtil.animate(
                              0,
                              l,
                              n,
                              function (e) {
                                  t.style.height = e + "px";
                              },
                              "linear",
                              function () {
                                  o(), (t.style.height = ""), (t.style.display = ""), (t.style.overflow = "");
                              }
                          ));
            }
        },
        slideUp: function (t, e, n) {
            mUtil.slide(t, "up", e, n);
        },
        slideDown: function (t, e, n) {
            mUtil.slide(t, "down", e, n);
        },
        show: function (t, e) {
            t.style.display = e || "block";
        },
        hide: function (t) {
            t.style.display = "none";
        },
        addEvent: function (t, e, n, o) {
            void 0 !== (t = mUtil.get(t)) && t.addEventListener(e, n);
        },
        removeEvent: function (t, e, n) {
            (t = mUtil.get(t)).removeEventListener(e, n);
        },
        on: function (t, e, n, o) {
            if (e) {
                var i = mUtil.getUniqueID("event");
                return (
                    (mUtilDelegatedEventHandlers[i] = function (n) {
                        for (var i = t.querySelectorAll(e), l = n.target; l && l !== t; ) {
                            for (var r = 0, s = i.length; r < s; r++) l === i[r] && o.call(l, n);
                            l = l.parentNode;
                        }
                    }),
                    mUtil.addEvent(t, n, mUtilDelegatedEventHandlers[i]),
                    i
                );
            }
        },
        off: function (t, e, n) {
            t && mUtilDelegatedEventHandlers[n] && (mUtil.removeEvent(t, e, mUtilDelegatedEventHandlers[n]), delete mUtilDelegatedEventHandlers[n]);
        },
        one: function (t, e, n) {
            (t = mUtil.get(t)).addEventListener(e, function (t) {
                return t.target.removeEventListener(t.type, arguments.callee), n(t);
            });
        },
        hash: function (t) {
            var e,
                n = 0;
            if (0 === t.length) return n;
            for (e = 0; e < t.length; e++) (n = (n << 5) - n + t.charCodeAt(e)), (n |= 0);
            return n;
        },
        animateClass: function (t, e, n) {
            mUtil.addClass(t, "animated " + e),
                mUtil.one(t, "webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend", function () {
                    mUtil.removeClass(t, "animated " + e);
                }),
                n && mUtil.one(t.animationEnd, n);
        },
        animateDelay: function (t, e) {
            for (var n = ["webkit-", "moz-", "ms-", "o-", ""], o = 0; o < n.length; o++) mUtil.css(t, n[o] + "animation-delay", e);
        },
        animateDuration: function (t, e) {
            for (var n = ["webkit-", "moz-", "ms-", "o-", ""], o = 0; o < n.length; o++) mUtil.css(t, n[o] + "animation-duration", e);
        },
        scrollTo: function (t, e, n) {
            n = n || 500;
            var o,
                i,
                l = (t = mUtil.get(t)) ? mUtil.offset(t).top : 0,
                r = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop || 0;
            l > r ? ((o = l), (i = r)) : ((o = r), (i = l)),
                e && (i += e),
                mUtil.animate(o, i, n, function (t) {
                    (document.documentElement.scrollTop = t), (document.body.parentNode.scrollTop = t), (document.body.scrollTop = t);
                });
        },
        scrollTop: function (t, e) {
            mUtil.scrollTo(null, t, e);
        },
        isArray: function (t) {
            return t && Array.isArray(t);
        },
        ready: function (t) {
            (document.attachEvent ? "complete" === document.readyState : "loading" !== document.readyState) ? t() : document.addEventListener("DOMContentLoaded", t);
        },
        isEmpty: function (t) {
            for (var e in t) if (t.hasOwnProperty(e)) return !1;
            return !0;
        },
        numberString: function (t) {
            for (var e = (t += "").split("."), n = e[0], o = e.length > 1 ? "." + e[1] : "", i = /(\d+)(\d{3})/; i.test(n); ) n = n.replace(i, "$1,$2");
            return n + o;
        },
        detectIE: function () {
            var t = window.navigator.userAgent,
                e = t.indexOf("MSIE ");
            if (e > 0) return parseInt(t.substring(e + 5, t.indexOf(".", e)), 10);
            if (t.indexOf("Trident/") > 0) {
                var n = t.indexOf("rv:");
                return parseInt(t.substring(n + 3, t.indexOf(".", n)), 10);
            }
            var o = t.indexOf("Edge/");
            return o > 0 && parseInt(t.substring(o + 5, t.indexOf(".", o)), 10);
        },
        isRTL: function () {
            return "rtl" == mUtil.attr(mUtil.get("html"), "direction");
        },
        scrollerInit: function (t, e) {
            function n() {
                var n, o;
                (o = e.height instanceof Function ? parseInt(e.height.call()) : parseInt(e.height)),
                    e.disableForMobile && mUtil.isInResponsiveRange("tablet-and-mobile")
                        ? (n = mUtil.data(t).get("ps"))
                            ? (e.resetHeightOnDestroy ? mUtil.css(t, "height", "auto") : (mUtil.css(t, "overflow", "auto"), o > 0 && mUtil.css(t, "height", o + "px")), n.destroy(), (n = mUtil.data(t).remove("ps")))
                            : o > 0 && (mUtil.css(t, "overflow", "auto"), mUtil.css(t, "height", o + "px"))
                        : (o > 0 && mUtil.css(t, "height", o + "px"),
                          mUtil.css(t, "overflow", "hidden"),
                          (n = mUtil.data(t).get("ps"))
                              ? n.update()
                              : (mUtil.addClass(t, "m-scroller"),
                                (n = new PerfectScrollbar(t, { wheelSpeed: 0.5, swipeEasing: !0, wheelPropagation: !1, minScrollbarLength: 40, suppressScrollX: !mUtil.isRTL() })),
                                mUtil.data(t).set("ps", n)));
            }
            n(),
                e.handleWindowResize &&
                    mUtil.addResizeHandler(function () {
                        n();
                    });
        },
        scrollerUpdate: function (t) {
            var e;
            (e = mUtil.data(t).get("ps")) && e.update();
        },
        scrollersUpdate: function (t) {
            for (var e = mUtil.findAll(t, ".ps"), n = 0, o = e.length; n < o; n++) mUtil.scrollerUpdate(e[n]);
        },
        scrollerTop: function (t) {
            mUtil.data(t).get("ps") && (t.scrollTop = 0);
        },
        scrollerDestroy: function (t) {
            var e;
            (e = mUtil.data(t).get("ps")) && (e.destroy(), (e = mUtil.data(t).remove("ps")));
        },
    };
})();
mUtil.ready(function () {
    mUtil.init();
});
var mHeader = function (t, e) {
        var n = this,
            o = mUtil.get(t),
            i = mUtil.get("body");
        if (void 0 !== o) {
            var l = { classic: !1, offset: { mobile: 150, desktop: 200 }, minimize: { mobile: !1, desktop: !1 } },
                r = {
                    construct: function (t) {
                        return mUtil.data(o).has("header") ? (n = mUtil.data(o).get("header")) : (r.init(t), r.build(), mUtil.data(o).set("header", n)), n;
                    },
                    init: function (t) {
                        (n.events = []), (n.options = mUtil.deepExtend({}, l, t));
                    },
                    build: function () {
                        var t = 0;
                        (!1 === n.options.minimize.mobile && !1 === n.options.minimize.desktop) ||
                            window.addEventListener("scroll", function () {
                                var e,
                                    o,
                                    l,
                                    r = 0;
                                mUtil.isInResponsiveRange("desktop")
                                    ? ((r = n.options.offset.desktop), (e = n.options.minimize.desktop.on), (o = n.options.minimize.desktop.off))
                                    : mUtil.isInResponsiveRange("tablet-and-mobile") && ((r = n.options.offset.mobile), (e = n.options.minimize.mobile.on), (o = n.options.minimize.mobile.off)),
                                    (l = window.pageYOffset),
                                    (mUtil.isInResponsiveRange("tablet-and-mobile") && n.options.classic && n.options.classic.mobile) || (mUtil.isInResponsiveRange("desktop") && n.options.classic && n.options.classic.desktop)
                                        ? l > r
                                            ? (mUtil.addClass(i, e), mUtil.removeClass(i, o))
                                            : (mUtil.addClass(i, o), mUtil.removeClass(i, e))
                                        : (l > r && t < l ? (mUtil.addClass(i, e), mUtil.removeClass(i, o)) : (mUtil.addClass(i, o), mUtil.removeClass(i, e)), (t = l));
                            });
                    },
                    eventTrigger: function (t, e) {
                        for (var o = 0; o < n.events.length; o++) {
                            var i = n.events[o];
                            i.name == t && (1 == i.one ? 0 == i.fired && ((n.events[o].fired = !0), i.handler.call(this, n, e)) : i.handler.call(this, n, e));
                        }
                    },
                    addEvent: function (t, e, o) {
                        n.events.push({ name: t, handler: e, one: o, fired: !1 });
                    },
                };
            return (
                (n.setDefaults = function (t) {
                    l = t;
                }),
                (n.on = function (t, e) {
                    return r.addEvent(t, e);
                }),
                r.construct.apply(n, [e]),
                !0,
                n
            );
        }
    },
    mMenu = function (t, e) {
        var n = this,
            o = !1,
            i = mUtil.get(t),
            l = mUtil.get("body");
        if (i) {
            var r = { accordion: { slideSpeed: 200, autoScroll: !1, autoScrollSpeed: 1200, expandAll: !0 }, dropdown: { timeout: 500 } },
                s = {
                    construct: function (t) {
                        return mUtil.data(i).has("menu") ? (n = mUtil.data(i).get("menu")) : (s.init(t), s.reset(), s.build(), mUtil.data(i).set("menu", n)), n;
                    },
                    init: function (t) {
                        (n.events = []), (n.eventHandlers = {}), (n.options = mUtil.deepExtend({}, r, t)), (n.pauseDropdownHoverTime = 0), (n.uid = mUtil.getUniqueID());
                    },
                    update: function (t) {
                        (n.options = mUtil.deepExtend({}, r, t)), (n.pauseDropdownHoverTime = 0), s.reset(), (n.eventHandlers = {}), s.build(), mUtil.data(i).set("menu", n);
                    },
                    reload: function () {
                        s.reset(), s.build();
                    },
                    build: function () {
                        (n.eventHandlers.event_1 = mUtil.on(i, ".m-menu__toggle", "click", s.handleSubmenuAccordion)),
                            ("dropdown" === s.getSubmenuMode() || s.isConditionalSubmenuDropdown()) &&
                                ((n.eventHandlers.event_2 = mUtil.on(i, '[m-menu-submenu-toggle="hover"]', "mouseover", s.handleSubmenuDrodownHoverEnter)),
                                (n.eventHandlers.event_3 = mUtil.on(i, '[m-menu-submenu-toggle="hover"]', "mouseout", s.handleSubmenuDrodownHoverExit)),
                                (n.eventHandlers.event_4 = mUtil.on(i, '[m-menu-submenu-toggle="click"] > .m-menu__toggle, [m-menu-submenu-toggle="click"] > .m-menu__link .m-menu__toggle', "click", s.handleSubmenuDropdownClick)),
                                (n.eventHandlers.event_5 = mUtil.on(i, '[m-menu-submenu-toggle="tab"] > .m-menu__toggle, [m-menu-submenu-toggle="tab"] > .m-menu__link .m-menu__toggle', "click", s.handleSubmenuDropdownTabClick))),
                            (n.eventHandlers.event_6 = mUtil.on(i, ".m-menu__item:not(.m-menu__item--submenu) > .m-menu__link:not(.m-menu__toggle):not(.m-menu__link--toggle-skip)", "click", s.handleLinkClick)),
                            n.options.scroll && n.options.scroll.height && s.scrollerInit();
                    },
                    reset: function () {
                        mUtil.off(i, "click", n.eventHandlers.event_1),
                            mUtil.off(i, "mouseover", n.eventHandlers.event_2),
                            mUtil.off(i, "mouseout", n.eventHandlers.event_3),
                            mUtil.off(i, "click", n.eventHandlers.event_4),
                            mUtil.off(i, "click", n.eventHandlers.event_5),
                            mUtil.off(i, "click", n.eventHandlers.event_6);
                    },
                    scrollerInit: function () {
                        n.options.scroll && n.options.scroll.height && (mUtil.scrollerDestroy(i), mUtil.scrollerInit(i, { disableForMobile: !0, resetHeightOnDestroy: !0, handleWindowResize: !0, height: n.options.scroll.height }));
                    },
                    scrollerUpdate: function () {
                        n.options.scroll && n.options.scroll.height ? mUtil.scrollerUpdate(i) : mUtil.scrollerDestroy(i);
                    },
                    scrollerTop: function () {
                        n.options.scroll && n.options.scroll.height && mUtil.scrollerTop(i);
                    },
                    getSubmenuMode: function (t) {
                        return mUtil.isInResponsiveRange("desktop")
                            ? t && mUtil.hasAttr(t, "m-menu-submenu-toggle")
                                ? mUtil.attr(t, "m-menu-submenu-toggle")
                                : mUtil.isset(n.options.submenu, "desktop.state.body")
                                ? mUtil.hasClass(l, n.options.submenu.desktop.state.body)
                                    ? n.options.submenu.desktop.state.mode
                                    : n.options.submenu.desktop.default
                                : mUtil.isset(n.options.submenu, "desktop")
                                ? n.options.submenu.desktop
                                : void 0
                            : mUtil.isInResponsiveRange("tablet") && mUtil.isset(n.options.submenu, "tablet")
                            ? n.options.submenu.tablet
                            : !(!mUtil.isInResponsiveRange("mobile") || !mUtil.isset(n.options.submenu, "mobile")) && n.options.submenu.mobile;
                    },
                    isConditionalSubmenuDropdown: function () {
                        return !(!mUtil.isInResponsiveRange("desktop") || !mUtil.isset(n.options.submenu, "desktop.state.body"));
                    },
                    handleLinkClick: function (t) {
                        !1 === s.eventTrigger("linkClick", this) && t.preventDefault(), ("dropdown" === s.getSubmenuMode(this) || s.isConditionalSubmenuDropdown()) && s.handleSubmenuDropdownClose(t, this);
                    },
                    handleSubmenuDrodownHoverEnter: function (t) {
                        if ("accordion" !== s.getSubmenuMode(this) && !1 !== n.resumeDropdownHover()) {
                            "1" == this.getAttribute("data-hover") && (this.removeAttribute("data-hover"), clearTimeout(this.getAttribute("data-timeout")), this.removeAttribute("data-timeout")), s.showSubmenuDropdown(this);
                        }
                    },
                    handleSubmenuDrodownHoverExit: function (t) {
                        if (!1 !== n.resumeDropdownHover() && "accordion" !== s.getSubmenuMode(this)) {
                            var e = this,
                                o = n.options.dropdown.timeout,
                                i = setTimeout(function () {
                                    "1" == e.getAttribute("data-hover") && s.hideSubmenuDropdown(e, !0);
                                }, o);
                            e.setAttribute("data-hover", "1"), e.setAttribute("data-timeout", i);
                        }
                    },
                    handleSubmenuDropdownClick: function (t) {
                        if ("accordion" !== s.getSubmenuMode(this)) {
                            var e = this.closest(".m-menu__item");
                            "accordion" != e.getAttribute("m-menu-submenu-mode") &&
                                (!1 === mUtil.hasClass(e, "m-menu__item--hover")
                                    ? (mUtil.addClass(e, "m-menu__item--open-dropdown"), s.showSubmenuDropdown(e))
                                    : (mUtil.removeClass(e, "m-menu__item--open-dropdown"), s.hideSubmenuDropdown(e, !0)),
                                t.preventDefault());
                        }
                    },
                    handleSubmenuDropdownTabClick: function (t) {
                        if ("accordion" !== s.getSubmenuMode(this)) {
                            var e = this.closest(".m-menu__item");
                            "accordion" != e.getAttribute("m-menu-submenu-mode") && (0 == mUtil.hasClass(e, "m-menu__item--hover") && (mUtil.addClass(e, "m-menu__item--open-dropdown"), s.showSubmenuDropdown(e)), t.preventDefault());
                        }
                    },
                    handleSubmenuDropdownClose: function (t, e) {
                        if ("accordion" !== s.getSubmenuMode(e)) {
                            var n = i.querySelectorAll(".m-menu__item.m-menu__item--submenu.m-menu__item--hover:not(.m-menu__item--tabs)");
                            if (n.length > 0 && !1 === mUtil.hasClass(e, "m-menu__toggle") && 0 === e.querySelectorAll(".m-menu__toggle").length) for (var o = 0, l = n.length; o < l; o++) s.hideSubmenuDropdown(n[0], !0);
                        }
                    },
                    handleSubmenuAccordion: function (t, e) {
                        var o,
                            i = e || this;
                        if ("dropdown" === s.getSubmenuMode(e) && (o = i.closest(".m-menu__item")) && "accordion" != o.getAttribute("m-menu-submenu-mode")) t.preventDefault();
                        else {
                            var l = i.closest(".m-menu__item"),
                                r = mUtil.child(l, ".m-menu__submenu, .m-menu__inner");
                            if (!mUtil.hasClass(i.closest(".m-menu__item"), "m-menu__item--open-always") && l && r) {
                                t.preventDefault();
                                var a = n.options.accordion.slideSpeed;
                                if (!1 === mUtil.hasClass(l, "m-menu__item--open")) {
                                    if (!1 === n.options.accordion.expandAll) {
                                        var m = i.closest(".m-menu__nav, .m-menu__subnav"),
                                            d = mUtil.children(m, ".m-menu__item.m-menu__item--open.m-menu__item--submenu:not(.m-menu__item--expanded):not(.m-menu__item--open-always)");
                                        if (m && d)
                                            for (var u = 0, c = d.length; u < c; u++) {
                                                var p = d[0],
                                                    f = mUtil.child(p, ".m-menu__submenu");
                                                f &&
                                                    mUtil.slideUp(f, a, function () {
                                                        s.scrollerUpdate(), mUtil.removeClass(p, "m-menu__item--open");
                                                    });
                                            }
                                    }
                                    mUtil.slideDown(r, a, function () {
                                        s.scrollToItem(i), s.scrollerUpdate(), s.eventTrigger("submenuToggle", r);
                                    }),
                                        mUtil.addClass(l, "m-menu__item--open");
                                } else
                                    mUtil.slideUp(r, a, function () {
                                        s.scrollToItem(i), s.eventTrigger("submenuToggle", r);
                                    }),
                                        mUtil.removeClass(l, "m-menu__item--open");
                            }
                        }
                    },
                    scrollToItem: function (t) {
                        mUtil.isInResponsiveRange("desktop") && n.options.accordion.autoScroll && "1" !== i.getAttribute("m-menu-scrollable") && mUtil.scrollTo(t, n.options.accordion.autoScrollSpeed);
                    },
                    hideSubmenuDropdown: function (t, e) {
                        e && (mUtil.removeClass(t, "m-menu__item--hover"), mUtil.removeClass(t, "m-menu__item--active-tab")),
                            t.removeAttribute("data-hover"),
                            t.getAttribute("m-menu-dropdown-toggle-class") && mUtil.removeClass(l, t.getAttribute("m-menu-dropdown-toggle-class"));
                        var n = t.getAttribute("data-timeout");
                        t.removeAttribute("data-timeout"), clearTimeout(n);
                    },
                    showSubmenuDropdown: function (t) {
                        var e = i.querySelectorAll(".m-menu__item--submenu.m-menu__item--hover, .m-menu__item--submenu.m-menu__item--active-tab");
                        if (e)
                            for (var n = 0, o = e.length; n < o; n++) {
                                var r = e[n];
                                t !== r && !1 === r.contains(t) && !1 === t.contains(r) && s.hideSubmenuDropdown(r, !0);
                            }
                        s.adjustSubmenuDropdownArrowPos(t), mUtil.addClass(t, "m-menu__item--hover"), t.getAttribute("m-menu-dropdown-toggle-class") && mUtil.addClass(l, t.getAttribute("m-menu-dropdown-toggle-class"));
                    },
                    createSubmenuDropdownClickDropoff: function (t) {
                        var e,
                            n = (e = mUtil.child(t, ".m-menu__submenu") ? mUtil.css(e, "z-index") : 0) - 1,
                            o = document.createElement('<div class="m-menu__dropoff" style="background: transparent; position: fixed; top: 0; bottom: 0; left: 0; right: 0; z-index: ' + n + '"></div>');
                        l.appendChild(o),
                            mUtil.addEvent(o, "click", function (e) {
                                e.stopPropagation(), e.preventDefault(), mUtil.remove(this), s.hideSubmenuDropdown(t, !0);
                            });
                    },
                    adjustSubmenuDropdownArrowPos: function (t) {
                        var e = mUtil.child(t, ".m-menu__submenu"),
                            n = mUtil.child(e, ".m-menu__arrow.m-menu__arrow--adjust");
                        mUtil.child(e, ".m-menu__subnav");
                        if (n) {
                            var o = 0;
                            mUtil.child(t, ".m-menu__link");
                            mUtil.hasClass(e, "m-menu__submenu--classic") || mUtil.hasClass(e, "m-menu__submenu--fixed")
                                ? (mUtil.hasClass(e, "m-menu__submenu--right")
                                      ? ((o = mUtil.outerWidth(t) / 2),
                                        mUtil.hasClass(e, "m-menu__submenu--pull") && (mUtil.isRTL() ? (o += Math.abs(parseFloat(mUtil.css(e, "margin-left")))) : (o += Math.abs(parseFloat(mUtil.css(e, "margin-right"))))),
                                        (o = parseInt(mUtil.css(e, "width")) - o))
                                      : mUtil.hasClass(e, "m-menu__submenu--left") &&
                                        ((o = mUtil.outerWidth(t) / 2),
                                        mUtil.hasClass(e, "m-menu__submenu--pull") && (mUtil.isRTL() ? (o += Math.abs(parseFloat(mUtil.css(e, "margin-right")))) : (o += Math.abs(parseFloat(mUtil.css(e, "margin-left")))))),
                                  mUtil.isRTL() ? mUtil.css(n, "right", o + "px") : mUtil.css(n, "left", o + "px"))
                                : (mUtil.hasClass(e, "m-menu__submenu--center") || mUtil.hasClass(e, "m-menu__submenu--full")) &&
                                  ((o = mUtil.offset(t).left - (mUtil.getViewPort().width - parseInt(mUtil.css(e, "width"))) / 2),
                                  (o += mUtil.outerWidth(t) / 2),
                                  mUtil.css(n, "left", o + "px"),
                                  mUtil.isRTL() && mUtil.css(n, "right", "auto"));
                        }
                    },
                    pauseDropdownHover: function (t) {
                        var e = new Date();
                        n.pauseDropdownHoverTime = e.getTime() + t;
                    },
                    resumeDropdownHover: function () {
                        return new Date().getTime() > n.pauseDropdownHoverTime;
                    },
                    resetActiveItem: function (t) {
                        for (var e, o, l = 0, r = (e = i.querySelectorAll(".m-menu__item--active")).length; l < r; l++) {
                            var s = e[0];
                            mUtil.removeClass(s, "m-menu__item--active"), mUtil.hide(mUtil.child(s, ".m-menu__submenu"));
                            for (var a = 0, m = (o = mUtil.parents(s, ".m-menu__item--submenu")).length; a < m; a++) {
                                var d = o[l];
                                mUtil.removeClass(d, "m-menu__item--open"), mUtil.hide(mUtil.child(d, ".m-menu__submenu"));
                            }
                        }
                        if (!1 === n.options.accordion.expandAll && (e = i.querySelectorAll(".m-menu__item--open"))) for (l = 0, r = e.length; l < r; l++) mUtil.removeClass(o[0], "m-menu__item--open");
                    },
                    setActiveItem: function (t) {
                        s.resetActiveItem(), mUtil.addClass(t, "m-menu__item--active");
                        for (var e = mUtil.parents(t, ".m-menu__item--submenu"), n = 0, o = e.length; n < o; n++) mUtil.addClass(e[n], "m-menu__item--open");
                    },
                    getBreadcrumbs: function (t) {
                        var e,
                            n = [],
                            o = mUtil.child(t, ".m-menu__link");
                        n.push({ text: (e = mUtil.child(o, ".m-menu__link-text") ? e.innerHTML : ""), title: o.getAttribute("title"), href: o.getAttribute("href") });
                        for (var i = mUtil.parents(t, ".m-menu__item--submenu"), l = 0, r = i.length; l < r; l++) {
                            var s = mUtil.child(i[l], ".m-menu__link");
                            n.push({ text: (e = mUtil.child(s, ".m-menu__link-text") ? e.innerHTML : ""), title: s.getAttribute("title"), href: s.getAttribute("href") });
                        }
                        return n.reverse();
                    },
                    getPageTitle: function (t) {
                        var e;
                        return mUtil.child(t, ".m-menu__link-text") ? e.innerHTML : "";
                    },
                    eventTrigger: function (t, e) {
                        for (var o = 0; o < n.events.length; o++) {
                            var i = n.events[o];
                            i.name == t && (1 == i.one ? 0 == i.fired && ((n.events[o].fired = !0), i.handler.call(this, n, e)) : i.handler.call(this, n, e));
                        }
                    },
                    addEvent: function (t, e, o) {
                        n.events.push({ name: t, handler: e, one: o, fired: !1 });
                    },
                    removeEvent: function (t) {
                        n.events[t] && delete n.events[t];
                    },
                };
            return (
                (n.setDefaults = function (t) {
                    r = t;
                }),
                (n.scrollerUpdate = function () {
                    return s.scrollerUpdate();
                }),
                (n.scrollerTop = function () {
                    return s.scrollerTop();
                }),
                (n.setActiveItem = function (t) {
                    return s.setActiveItem(t);
                }),
                (n.reload = function () {
                    return s.reload();
                }),
                (n.update = function (t) {
                    return s.update(t);
                }),
                (n.getBreadcrumbs = function (t) {
                    return s.getBreadcrumbs(t);
                }),
                (n.getPageTitle = function (t) {
                    return s.getPageTitle(t);
                }),
                (n.getSubmenuMode = function (t) {
                    return s.getSubmenuMode(t);
                }),
                (n.hideDropdown = function (t) {
                    s.hideSubmenuDropdown(t, !0);
                }),
                (n.pauseDropdownHover = function (t) {
                    s.pauseDropdownHover(t);
                }),
                (n.resumeDropdownHover = function () {
                    return s.resumeDropdownHover();
                }),
                (n.on = function (t, e) {
                    return s.addEvent(t, e);
                }),
                (n.off = function (t) {
                    return s.removeEvent(t);
                }),
                (n.one = function (t, e) {
                    return s.addEvent(t, e, !0);
                }),
                s.construct.apply(n, [e]),
                mUtil.addResizeHandler(function () {
                    o && n.reload();
                }),
                (o = !0),
                n
            );
        }
    };
document.addEventListener("click", function (t) {
    var e;
    if ((e = mUtil.get("body").querySelectorAll('.m-menu__nav .m-menu__item.m-menu__item--submenu.m-menu__item--hover:not(.m-menu__item--tabs)[m-menu-submenu-toggle="click"]')))
        for (var n = 0, o = e.length; n < o; n++) {
            var i = e[n].closest(".m-menu__nav").parentNode;
            if (i) {
                var l,
                    r = mUtil.data(i).get("menu");
                if (!r) break;
                if (!r || "dropdown" !== r.getSubmenuMode()) break;
                if (t.target !== i && !1 === i.contains(t.target))
                    if ((l = i.querySelectorAll('.m-menu__item--submenu.m-menu__item--hover:not(.m-menu__item--tabs)[m-menu-submenu-toggle="click"]'))) for (var s = 0, a = l.length; s < a; s++) r.hideDropdown(l[s]);
            }
        }
});
var mDropdown = function (t, e) {
    var n = this,
        o = mUtil.get(t),
        i = mUtil.get("body");
    if (o) {
        var l = { toggle: "click", hoverTimeout: 300, skin: "light", height: "auto", maxHeight: !1, minHeight: !1, persistent: !1, mobileOverlay: !0 },
            r = {
                construct: function (t) {
                    return mUtil.data(o).has("dropdown") ? (n = mUtil.data(o).get("dropdown")) : (r.init(t), r.setup(), mUtil.data(o).set("dropdown", n)), n;
                },
                init: function (t) {
                    (n.options = mUtil.deepExtend({}, l, t)),
                        (n.events = []),
                        (n.eventHandlers = {}),
                        (n.open = !1),
                        (n.layout = {}),
                        (n.layout.close = mUtil.find(o, ".m-dropdown__close")),
                        (n.layout.toggle = mUtil.find(o, ".m-dropdown__toggle")),
                        (n.layout.arrow = mUtil.find(o, ".m-dropdown__arrow")),
                        (n.layout.wrapper = mUtil.find(o, ".m-dropdown__wrapper")),
                        (n.layout.defaultDropPos = mUtil.hasClass(o, "m-dropdown--up") ? "up" : "down"),
                        (n.layout.currentDropPos = n.layout.defaultDropPos),
                        "hover" == mUtil.attr(o, "m-dropdown-toggle") && (n.options.toggle = "hover");
                },
                setup: function () {
                    n.options.placement && mUtil.addClass(o, "m-dropdown--" + n.options.placement),
                        n.options.align && mUtil.addClass(o, "m-dropdown--align-" + n.options.align),
                        n.options.width && mUtil.css(n.layout.wrapper, "width", n.options.width + "px"),
                        "1" == mUtil.attr(o, "m-dropdown-persistent") && (n.options.persistent = !0),
                        "hover" == n.options.toggle && mUtil.addEvent(o, "mouseout", r.hideMouseout),
                        r.setZindex();
                },
                toggle: function () {
                    return n.open ? r.hide() : r.show();
                },
                setContent: function (t) {
                    t = mUtil.find(o, ".m-dropdown__content").innerHTML = t;
                    return n;
                },
                show: function () {
                    if ("hover" == n.options.toggle && mUtil.hasAttr(o, "hover")) return r.clearHovered(), n;
                    if (n.open) return n;
                    if ((n.layout.arrow && r.adjustArrowPos(), r.eventTrigger("beforeShow"), r.hideOpened(), mUtil.addClass(o, "m-dropdown--open"), mUtil.isMobileDevice() && n.options.mobileOverlay)) {
                        var t = mUtil.css(o, "z-index") - 1,
                            e = mUtil.insertAfter(document.createElement("DIV"), o);
                        mUtil.addClass(e, "m-dropdown__dropoff"),
                            mUtil.css(e, "z-index", t),
                            mUtil.data(e).set("dropdown", o),
                            mUtil.data(o).set("dropoff", e),
                            mUtil.addEvent(e, "click", function (t) {
                                r.hide(), mUtil.remove(this), t.preventDefault();
                            });
                    }
                    return o.focus(), o.setAttribute("aria-expanded", "true"), (n.open = !0), mUtil.scrollersUpdate(o), r.eventTrigger("afterShow"), n;
                },
                clearHovered: function () {
                    var t = mUtil.attr(o, "timeout");
                    mUtil.removeAttr(o, "hover"), mUtil.removeAttr(o, "timeout"), clearTimeout(t);
                },
                hideHovered: function (t) {
                    if (!0 === t) {
                        if (!1 === r.eventTrigger("beforeHide")) return;
                        r.clearHovered(), mUtil.removeClass(o, "m-dropdown--open"), (n.open = !1), r.eventTrigger("afterHide");
                    } else {
                        if (!0 === mUtil.hasAttr(o, "hover")) return;
                        if (!1 === r.eventTrigger("beforeHide")) return;
                        var e = setTimeout(function () {
                            mUtil.attr(o, "hover") && (r.clearHovered(), mUtil.removeClass(o, "m-dropdown--open"), (n.open = !1), r.eventTrigger("afterHide"));
                        }, n.options.hoverTimeout);
                        mUtil.attr(o, "hover", "1"), mUtil.attr(o, "timeout", e);
                    }
                },
                hideClicked: function () {
                    !1 !== r.eventTrigger("beforeHide") && (mUtil.removeClass(o, "m-dropdown--open"), mUtil.data(o).remove("dropoff"), (n.open = !1), r.eventTrigger("afterHide"));
                },
                hide: function (t) {
                    return !1 === n.open
                        ? n
                        : (mUtil.isDesktopDevice() && "hover" == n.options.toggle ? r.hideHovered(t) : r.hideClicked(),
                          "down" == n.layout.defaultDropPos && "up" == n.layout.currentDropPos && (mUtil.removeClass(o, "m-dropdown--up"), n.layout.arrow.prependTo(n.layout.wrapper), (n.layout.currentDropPos = "down")),
                          n);
                },
                hideMouseout: function () {
                    mUtil.isDesktopDevice() && r.hide();
                },
                hideOpened: function () {
                    for (var t = mUtil.findAll(i, ".m-dropdown.m-dropdown--open"), e = 0, n = t.length; e < n; e++) {
                        var o = t[e];
                        mUtil.data(o).get("dropdown").hide(!0);
                    }
                },
                adjustArrowPos: function () {
                    var t = mUtil.outerWidth(o),
                        e = mUtil.hasClass(n.layout.arrow, "m-dropdown__arrow--right") ? "right" : "left",
                        i = 0;
                    n.layout.arrow &&
                        (mUtil.isInResponsiveRange("mobile") && mUtil.hasClass(o, "m-dropdown--mobile-full-width")
                            ? ((i = mUtil.offset(o).left + t / 2 - Math.abs(parseInt(mUtil.css(n.layout.arrow, "width")) / 2) - parseInt(mUtil.css(n.layout.wrapper, "left"))),
                              mUtil.css(n.layout.arrow, "right", "auto"),
                              mUtil.css(n.layout.arrow, "left", i + "px"),
                              mUtil.css(n.layout.arrow, "margin-left", "auto"),
                              mUtil.css(n.layout.arrow, "margin-right", "auto"))
                            : mUtil.hasClass(n.layout.arrow, "m-dropdown__arrow--adjust") &&
                              ((i = t / 2 - Math.abs(parseInt(mUtil.css(n.layout.arrow, "width")) / 2)),
                              mUtil.hasClass(o, "m-dropdown--align-push") && (i += 20),
                              "right" == e
                                  ? mUtil.isRTL()
                                      ? (mUtil.css(n.layout.arrow, "right", "auto"), mUtil.css(n.layout.arrow, "left", i + "px"))
                                      : (mUtil.css(n.layout.arrow, "left", "auto"), mUtil.css(n.layout.arrow, "right", i + "px"))
                                  : mUtil.isRTL()
                                  ? (mUtil.css(n.layout.arrow, "left", "auto"), mUtil.css(n.layout.arrow, "right", i + "px"))
                                  : (mUtil.css(n.layout.arrow, "right", "auto"), mUtil.css(n.layout.arrow, "left", i + "px"))));
                },
                setZindex: function () {
                    var t = 101,
                        e = mUtil.getHighestZindex(o);
                    e >= t && (t = e + 1), mUtil.css(n.layout.wrapper, "z-index", t);
                },
                isPersistent: function () {
                    return n.options.persistent;
                },
                isShown: function () {
                    return n.open;
                },
                eventTrigger: function (t, e) {
                    for (var o = 0; o < n.events.length; o++) {
                        var i = n.events[o];
                        i.name == t && (1 == i.one ? 0 == i.fired && ((n.events[o].fired = !0), i.handler.call(this, n, e)) : i.handler.call(this, n, e));
                    }
                },
                addEvent: function (t, e, o) {
                    n.events.push({ name: t, handler: e, one: o, fired: !1 });
                },
            };
        return (
            (n.setDefaults = function (t) {
                l = t;
            }),
            (n.show = function () {
                return r.show();
            }),
            (n.hide = function () {
                return r.hide();
            }),
            (n.toggle = function () {
                return r.toggle();
            }),
            (n.isPersistent = function () {
                return r.isPersistent();
            }),
            (n.isShown = function () {
                return r.isShown();
            }),
            (n.setContent = function (t) {
                return r.setContent(t);
            }),
            (n.on = function (t, e) {
                return r.addEvent(t, e);
            }),
            (n.one = function (t, e) {
                return r.addEvent(t, e, !0);
            }),
            r.construct.apply(n, [e]),
            !0,
            n
        );
    }
};
mUtil.on(document, '[m-dropdown-toggle="click"] .m-dropdown__toggle', "click", function (t) {
    var e = this.closest(".m-dropdown");
    e && ((mUtil.data(e).has("dropdown") ? mUtil.data(e).get("dropdown") : new mDropdown(e)).toggle(), t.preventDefault());
}),
    mUtil.on(document, '[m-dropdown-toggle="hover"] .m-dropdown__toggle', "click", function (t) {
        if (mUtil.isDesktopDevice()) "#" == mUtil.attr(this, "href") && t.preventDefault();
        else if (mUtil.isMobileDevice()) {
            var e = this.closest(".m-dropdown");
            e && ((mUtil.data(e).has("dropdown") ? mUtil.data(e).get("dropdown") : new mDropdown(e)).toggle(), t.preventDefault());
        }
    }),
    mUtil.on(document, '[m-dropdown-toggle="hover"]', "mouseover", function (t) {
        if (mUtil.isDesktopDevice()) {
            this && ((mUtil.data(this).has("dropdown") ? mUtil.data(this).get("dropdown") : new mDropdown(this)).show(), t.preventDefault());
        }
    }),
    document.addEventListener("click", function (t) {
        var e,
            n = mUtil.get("body"),
            o = t.target;
        if ((e = n.querySelectorAll(".m-dropdown.m-dropdown--open")))
            for (var i = 0, l = e.length; i < l; i++) {
                var r = e[i];
                if (!1 === mUtil.data(r).has("dropdown")) return;
                var s = mUtil.data(r).get("dropdown"),
                    a = mUtil.find(r, ".m-dropdown__toggle");
                mUtil.hasClass(r, "m-dropdown--disable-close") && (t.preventDefault(), t.stopPropagation()),
                    a && o !== a && !1 === a.contains(o) && !1 === o.contains(a) ? !1 === s.isPersistent() && s.hide() : !1 === r.contains(o) && s.hide();
            }
    });
var mOffcanvas = function (t, e) {
        var n = this,
            o = mUtil.get(t),
            i = mUtil.get("body");
        if (o) {
            var l = {},
                r = {
                    construct: function (t) {
                        return mUtil.data(o).has("offcanvas") ? (n = mUtil.data(o).get("offcanvas")) : (r.init(t), r.build(), mUtil.data(o).set("offcanvas", n)), n;
                    },
                    init: function (t) {
                        (n.events = []),
                            (n.options = mUtil.deepExtend({}, l, t)),
                            n.overlay,
                            (n.classBase = n.options.baseClass),
                            (n.classShown = n.classBase + "--on"),
                            (n.classOverlay = n.classBase + "-overlay"),
                            (n.state = mUtil.hasClass(o, n.classShown) ? "shown" : "hidden");
                    },
                    build: function () {
                        if (n.options.toggleBy)
                            if ("string" == typeof n.options.toggleBy) mUtil.addEvent(n.options.toggleBy, "click", r.toggle);
                            else if (n.options.toggleBy && n.options.toggleBy[0] && n.options.toggleBy[0].target) for (var t in n.options.toggleBy) mUtil.addEvent(n.options.toggleBy[t].target, "click", r.toggle);
                            else n.options.toggleBy && n.options.toggleBy.target && mUtil.addEvent(n.options.toggleBy.target, "click", r.toggle);
                        var e = mUtil.get(n.options.closeBy);
                        e && mUtil.addEvent(e, "click", r.hide);
                    },
                    toggle: function () {
                        r.eventTrigger("toggle"), "shown" == n.state ? r.hide(this) : r.show(this);
                    },
                    show: function (t) {
                        "shown" != n.state &&
                            (r.eventTrigger("beforeShow"),
                            r.togglerClass(t, "show"),
                            mUtil.addClass(i, n.classShown),
                            mUtil.addClass(o, n.classShown),
                            (n.state = "shown"),
                            n.options.overlay &&
                                ((n.overlay = mUtil.insertAfter(document.createElement("DIV"), o)),
                                mUtil.addClass(n.overlay, n.classOverlay),
                                mUtil.addEvent(n.overlay, "click", function (e) {
                                    e.stopPropagation(), e.preventDefault(), r.hide(t);
                                })),
                            r.eventTrigger("afterShow"));
                    },
                    hide: function (t) {
                        "hidden" != n.state &&
                            (r.eventTrigger("beforeHide"),
                            r.togglerClass(t, "hide"),
                            mUtil.removeClass(i, n.classShown),
                            mUtil.removeClass(o, n.classShown),
                            (n.state = "hidden"),
                            n.options.overlay && n.overlay && mUtil.remove(n.overlay),
                            r.eventTrigger("afterHide"));
                    },
                    togglerClass: function (t, e) {
                        var o,
                            i = mUtil.attr(t, "id");
                        if (n.options.toggleBy && n.options.toggleBy[0] && n.options.toggleBy[0].target) for (var l in n.options.toggleBy) n.options.toggleBy[l].target === i && (o = n.options.toggleBy[l]);
                        else n.options.toggleBy && n.options.toggleBy.target && (o = n.options.toggleBy);
                        if (o) {
                            var r = mUtil.get(o.target);
                            "show" === e && mUtil.addClass(r, o.state), "hide" === e && mUtil.removeClass(r, o.state);
                        }
                    },
                    eventTrigger: function (t, e) {
                        for (var o = 0; o < n.events.length; o++) {
                            var i = n.events[o];
                            i.name == t && (1 == i.one ? 0 == i.fired && ((n.events[o].fired = !0), i.handler.call(this, n, e)) : i.handler.call(this, n, e));
                        }
                    },
                    addEvent: function (t, e, o) {
                        n.events.push({ name: t, handler: e, one: o, fired: !1 });
                    },
                };
            return (
                (n.setDefaults = function (t) {
                    l = t;
                }),
                (n.hide = function () {
                    return r.hide();
                }),
                (n.show = function () {
                    return r.show();
                }),
                (n.on = function (t, e) {
                    return r.addEvent(t, e);
                }),
                (n.one = function (t, e) {
                    return r.addEvent(t, e, !0);
                }),
                r.construct.apply(n, [e]),
                !0,
                n
            );
        }
    },
    mPortlet = function (t, e) {
        var n = this,
            o = mUtil.get(t),
            l = mUtil.get("body");
        if (o) {
            var r = {
                    bodyToggleSpeed: 400,
                    tooltips: !0,
                    tools: { toggle: { collapse: "Collapse", expand: "Expand" }, reload: "Reload", remove: "Remove", fullscreen: { on: "Fullscreen", off: "Exit Fullscreen" } },
                    sticky: { offset: 300, zIndex: 98 },
                },
                s = {
                    construct: function (t) {
                        return mUtil.data(o).has("portlet") ? (n = mUtil.data(o).get("portlet")) : (s.init(t), s.build(), mUtil.data(o).set("portlet", n)), n;
                    },
                    init: function (t) {
                        (n.element = o),
                            (n.events = []),
                            (n.options = mUtil.deepExtend({}, r, t)),
                            (n.head = mUtil.child(o, ".m-portlet__head")),
                            (n.foot = mUtil.child(o, ".m-portlet__foot")),
                            mUtil.child(o, ".m-portlet__body") ? (n.body = mUtil.child(o, ".m-portlet__body")) : 0 !== mUtil.child(o, ".m-form").length && (n.body = mUtil.child(o, ".m-form"));
                    },
                    build: function () {
                        var t = mUtil.find(n.head, "[m-portlet-tool=remove]");
                        t &&
                            mUtil.addEvent(t, "click", function (t) {
                                t.preventDefault(), s.remove();
                            });
                        var e = mUtil.find(n.head, "[m-portlet-tool=reload]");
                        e &&
                            mUtil.addEvent(e, "click", function (t) {
                                t.preventDefault(), s.reload();
                            });
                        var o = mUtil.find(n.head, "[m-portlet-tool=toggle]");
                        o &&
                            mUtil.addEvent(o, "click", function (t) {
                                t.preventDefault(), s.toggle();
                            });
                        var i = mUtil.find(n.head, "[m-portlet-tool=fullscreen]");
                        i &&
                            mUtil.addEvent(i, "click", function (t) {
                                t.preventDefault(), s.fullscreen();
                            }),
                            s.setupTooltips();
                    },
                    onScrollSticky: function () {
                        window.pageYOffset > n.options.sticky.offset
                            ? !1 === mUtil.hasClass(l, "m-portlet--sticky") && (s.eventTrigger("stickyOn"), mUtil.addClass(l, "m-portlet--sticky"), mUtil.addClass(o, "m-portlet--sticky"), s.updateSticky())
                            : mUtil.hasClass(l, "m-portlet--sticky") && (s.eventTrigger("stickyOff"), mUtil.removeClass(l, "m-portlet--sticky"), mUtil.removeClass(o, "m-portlet--sticky"), s.resetSticky());
                    },
                    initSticky: function () {
                        n.head && window.addEventListener("scroll", s.onScrollSticky);
                    },
                    updateSticky: function () {
                        var t, e, o;
                        n.head &&
                            mUtil.hasClass(l, "m-portlet--sticky") &&
                            ((t = n.options.sticky.position.top instanceof Function ? parseInt(n.options.sticky.position.top.call()) : parseInt(n.options.sticky.position.top)),
                            (e = n.options.sticky.position.left instanceof Function ? parseInt(n.options.sticky.position.left.call()) : parseInt(n.options.sticky.position.left)),
                            (o = n.options.sticky.position.right instanceof Function ? parseInt(n.options.sticky.position.right.call()) : parseInt(n.options.sticky.position.right)),
                            mUtil.css(n.head, "z-index", n.options.sticky.zIndex),
                            mUtil.css(n.head, "top", t + "px"),
                            mUtil.isRTL() ? (mUtil.css(n.head, "left", o + "px"), mUtil.css(n.head, "right", e + "px")) : (mUtil.css(n.head, "left", e + "px"), mUtil.css(n.head, "right", o + "px")));
                    },
                    resetSticky: function () {
                        n.head && !1 === mUtil.hasClass(l, "m-portlet--sticky") && (mUtil.css(n.head, "z-index", ""), mUtil.css(n.head, "top", ""), mUtil.css(n.head, "left", ""), mUtil.css(n.head, "right", ""));
                    },
                    destroySticky: function () {
                        n.head && (s.resetSticky(), window.removeEventListener("scroll", s.onScrollSticky));
                    },
                    remove: function () {
                        !1 !== s.eventTrigger("beforeRemove") &&
                            (mUtil.hasClass(l, "m-portlet--fullscreen") && mUtil.hasClass(o, "m-portlet--fullscreen") && s.fullscreen("off"), s.removeTooltips(), mUtil.remove(o), s.eventTrigger("afterRemove"));
                    },
                    setContent: function (t) {
                        t && (n.body.innerHTML = t);
                    },
                    getBody: function () {
                        return n.body;
                    },
                    getSelf: function () {
                        return o;
                    },
                    setupTooltips: function () {
                        if (n.options.tooltips) {
                            var t = mUtil.hasClass(o, "m-portlet--collapse") || mUtil.hasClass(o, "m-portlet--collapsed"),
                                e = mUtil.hasClass(l, "m-portlet--fullscreen") && mUtil.hasClass(o, "m-portlet--fullscreen"),
                                i = mUtil.find(n.head, "[m-portlet-tool=remove]");
                            if (i) {
                                var r = e ? "bottom" : "top",
                                    s = new Tooltip(i, {
                                        title: n.options.tools.remove,
                                        placement: r,
                                        offset: e ? "0,10px,0,0" : "0,5px",
                                        trigger: "hover",
                                        template:
                                            '<div class="m-tooltip m-tooltip--portlet tooltip bs-tooltip-' +
                                            r +
                                            '" role="tooltip">                            <div class="tooltip-arrow arrow"></div>                            <div class="tooltip-inner"></div>                        </div>',
                                    });
                                mUtil.data(i).set("tooltip", s);
                            }
                            var a = mUtil.find(n.head, "[m-portlet-tool=reload]");
                            if (a) {
                                (r = e ? "bottom" : "top"),
                                    (s = new Tooltip(a, {
                                        title: n.options.tools.reload,
                                        placement: r,
                                        offset: e ? "0,10px,0,0" : "0,5px",
                                        trigger: "hover",
                                        template:
                                            '<div class="m-tooltip m-tooltip--portlet tooltip bs-tooltip-' +
                                            r +
                                            '" role="tooltip">                            <div class="tooltip-arrow arrow"></div>                            <div class="tooltip-inner"></div>                        </div>',
                                    }));
                                mUtil.data(a).set("tooltip", s);
                            }
                            var m = mUtil.find(n.head, "[m-portlet-tool=toggle]");
                            if (m) {
                                (r = e ? "bottom" : "top"),
                                    (s = new Tooltip(m, {
                                        title: t ? n.options.tools.toggle.expand : n.options.tools.toggle.collapse,
                                        placement: r,
                                        offset: e ? "0,10px,0,0" : "0,5px",
                                        trigger: "hover",
                                        template:
                                            '<div class="m-tooltip m-tooltip--portlet tooltip bs-tooltip-' +
                                            r +
                                            '" role="tooltip">                            <div class="tooltip-arrow arrow"></div>                            <div class="tooltip-inner"></div>                        </div>',
                                    }));
                                mUtil.data(m).set("tooltip", s);
                            }
                            var d = mUtil.find(n.head, "[m-portlet-tool=fullscreen]");
                            if (d) {
                                (r = e ? "bottom" : "top"),
                                    (s = new Tooltip(d, {
                                        title: e ? n.options.tools.fullscreen.off : n.options.tools.fullscreen.on,
                                        placement: r,
                                        offset: e ? "0,10px,0,0" : "0,5px",
                                        trigger: "hover",
                                        template:
                                            '<div class="m-tooltip m-tooltip--portlet tooltip bs-tooltip-' +
                                            r +
                                            '" role="tooltip">                            <div class="tooltip-arrow arrow"></div>                            <div class="tooltip-inner"></div>                        </div>',
                                    }));
                                mUtil.data(d).set("tooltip", s);
                            }
                        }
                    },
                    removeTooltips: function () {
                        if (n.options.tooltips) {
                            var t = mUtil.find(n.head, "[m-portlet-tool=remove]");
                            t && mUtil.data(t).has("tooltip") && mUtil.data(t).get("tooltip").dispose();
                            var e = mUtil.find(n.head, "[m-portlet-tool=reload]");
                            e && mUtil.data(e).has("tooltip") && mUtil.data(e).get("tooltip").dispose();
                            var o = mUtil.find(n.head, "[m-portlet-tool=toggle]");
                            o && mUtil.data(o).has("tooltip") && mUtil.data(o).get("tooltip").dispose();
                            var i = mUtil.find(n.head, "[m-portlet-tool=fullscreen]");
                            i && mUtil.data(i).has("tooltip") && mUtil.data(i).get("tooltip").dispose();
                        }
                    },
                    reload: function () {
                        s.eventTrigger("reload");
                    },
                    toggle: function () {
                        mUtil.hasClass(o, "m-portlet--collapse") || mUtil.hasClass(o, "m-portlet--collapsed") ? s.expand() : s.collapse();
                    },
                    collapse: function () {
                        if (!1 !== s.eventTrigger("beforeCollapse")) {
                            mUtil.slideUp(n.body, n.options.bodyToggleSpeed, function () {
                                s.eventTrigger("afterCollapse");
                            }),
                                mUtil.addClass(o, "m-portlet--collapse");
                            var t = mUtil.find(n.head, "[m-portlet-tool=toggle]");
                            t && mUtil.data(t).has("tooltip") && mUtil.data(t).get("tooltip").updateTitleContent(n.options.tools.toggle.expand);
                        }
                    },
                    expand: function () {
                        if (!1 !== s.eventTrigger("beforeExpand")) {
                            mUtil.slideDown(n.body, n.options.bodyToggleSpeed, function () {
                                s.eventTrigger("afterExpand");
                            }),
                                mUtil.removeClass(o, "m-portlet--collapse"),
                                mUtil.removeClass(o, "m-portlet--collapsed");
                            var t = mUtil.find(n.head, "[m-portlet-tool=toggle]");
                            t && mUtil.data(t).has("tooltip") && mUtil.data(t).get("tooltip").updateTitleContent(n.options.tools.toggle.collapse);
                        }
                    },
                    fullscreen: function (t) {
                        if ("off" === t || (mUtil.hasClass(l, "m-portlet--fullscreen") && mUtil.hasClass(o, "m-portlet--fullscreen")))
                            s.eventTrigger("beforeFullscreenOff"),
                                mUtil.removeClass(l, "m-portlet--fullscreen"),
                                mUtil.removeClass(o, "m-portlet--fullscreen"),
                                s.removeTooltips(),
                                s.setupTooltips(),
                                n.foot && (mUtil.css(n.body, "margin-bottom", ""), mUtil.css(n.foot, "margin-top", "")),
                                s.eventTrigger("afterFullscreenOff");
                        else {
                            if ((s.eventTrigger("beforeFullscreenOn"), mUtil.addClass(o, "m-portlet--fullscreen"), mUtil.addClass(l, "m-portlet--fullscreen"), s.removeTooltips(), s.setupTooltips(), n.foot)) {
                                var e = parseInt(mUtil.css(n.foot, "height")),
                                    i = parseInt(mUtil.css(n.foot, "height")) + parseInt(mUtil.css(n.head, "height"));
                                mUtil.css(n.body, "margin-bottom", e + "px"), mUtil.css(n.foot, "margin-top", "-" + i + "px");
                            }
                            s.eventTrigger("afterFullscreenOn");
                        }
                    },
                    eventTrigger: function (t) {
                        for (i = 0; i < n.events.length; i++) {
                            var e = n.events[i];
                            e.name == t && (1 == e.one ? 0 == e.fired && ((n.events[i].fired = !0), e.handler.call(this, n)) : e.handler.call(this, n));
                        }
                    },
                    addEvent: function (t, e, o) {
                        return n.events.push({ name: t, handler: e, one: o, fired: !1 }), n;
                    },
                };
            return (
                (n.setDefaults = function (t) {
                    r = t;
                }),
                (n.remove = function () {
                    return s.remove(html);
                }),
                (n.initSticky = function () {
                    return s.initSticky();
                }),
                (n.updateSticky = function () {
                    return s.updateSticky();
                }),
                (n.resetSticky = function () {
                    return s.resetSticky();
                }),
                (n.destroySticky = function () {
                    return s.destroySticky();
                }),
                (n.reload = function () {
                    return s.reload();
                }),
                (n.setContent = function (t) {
                    return s.setContent(t);
                }),
                (n.toggle = function () {
                    return s.toggle();
                }),
                (n.collapse = function () {
                    return s.collapse();
                }),
                (n.expand = function () {
                    return s.expand();
                }),
                (n.fullscreen = function () {
                    return s.fullscreen("on");
                }),
                (n.unFullscreen = function () {
                    return s.fullscreen("off");
                }),
                (n.getBody = function () {
                    return s.getBody();
                }),
                (n.getSelf = function () {
                    return s.getSelf();
                }),
                (n.on = function (t, e) {
                    return s.addEvent(t, e);
                }),
                (n.one = function (t, e) {
                    return s.addEvent(t, e, !0);
                }),
                s.construct.apply(n, [e]),
                n
            );
        }
    },
    mToggle = function (t, e) {
        var n = this,
            o = mUtil.get(t);
        mUtil.get("body");
        if (o) {
            var l = { togglerState: "", targetState: "" },
                r = {
                    construct: function (t) {
                        return mUtil.data(o).has("toggle") ? (n = mUtil.data(o).get("toggle")) : (r.init(t), r.build(), mUtil.data(o).set("toggle", n)), n;
                    },
                    init: function (t) {
                        (n.element = o),
                            (n.events = []),
                            (n.options = mUtil.deepExtend({}, l, t)),
                            (n.target = mUtil.get(n.options.target)),
                            (n.targetState = n.options.targetState),
                            (n.togglerState = n.options.togglerState),
                            (n.state = mUtil.hasClasses(n.target, n.targetState) ? "on" : "off");
                    },
                    build: function () {
                        mUtil.addEvent(o, "mouseup", r.toggle);
                    },
                    toggle: function () {
                        return r.eventTrigger("beforeToggle"), "off" == n.state ? r.toggleOn() : r.toggleOff(), n;
                    },
                    toggleOn: function () {
                        return r.eventTrigger("beforeOn"), mUtil.addClass(n.target, n.targetState), n.togglerState && mUtil.addClass(o, n.togglerState), (n.state = "on"), r.eventTrigger("afterOn"), r.eventTrigger("toggle"), n;
                    },
                    toggleOff: function () {
                        return r.eventTrigger("beforeOff"), mUtil.removeClass(n.target, n.targetState), n.togglerState && mUtil.removeClass(o, n.togglerState), (n.state = "off"), r.eventTrigger("afterOff"), r.eventTrigger("toggle"), n;
                    },
                    eventTrigger: function (t) {
                        for (i = 0; i < n.events.length; i++) {
                            var e = n.events[i];
                            e.name == t && (1 == e.one ? 0 == e.fired && ((n.events[i].fired = !0), e.handler.call(this, n)) : e.handler.call(this, n));
                        }
                    },
                    addEvent: function (t, e, o) {
                        return n.events.push({ name: t, handler: e, one: o, fired: !1 }), n;
                    },
                };
            return (
                (n.setDefaults = function (t) {
                    l = t;
                }),
                (n.getState = function () {
                    return n.state;
                }),
                (n.toggle = function () {
                    return r.toggle();
                }),
                (n.toggleOn = function () {
                    return r.toggleOn();
                }),
                (n.toggle = function () {
                    return r.toggleOff();
                }),
                (n.on = function (t, e) {
                    return r.addEvent(t, e);
                }),
                (n.one = function (t, e) {
                    return r.addEvent(t, e, !0);
                }),
                r.construct.apply(n, [e]),
                n
            );
        }
    },
    mQuicksearch = function (t, e) {
        var n = this,
            o = mUtil.get(t),
            l = mUtil.get("body");
        if (o) {
            var r = {
                    mode: "default",
                    minLength: 1,
                    maxHeight: 300,
                    requestTimeout: 200,
                    inputTarget: "m_quicksearch_input",
                    iconCloseTarget: "m_quicksearch_close",
                    iconCancelTarget: "m_quicksearch_cancel",
                    iconSearchTarget: "m_quicksearch_search",
                    spinnerClass: "m-loader m-loader--skin-light m-loader--right",
                    hasResultClass: "m-list-search--has-result",
                    templates: { error: '<div class="m-search-results m-search-results--skin-light"><span class="m-search-result__message">{{message}}</div></div>' },
                },
                s = {
                    construct: function (t) {
                        return mUtil.data(o).has("quicksearch") ? (n = mUtil.data(o).get("quicksearch")) : (s.init(t), s.build(), mUtil.data(o).set("quicksearch", n)), n;
                    },
                    init: function (t) {
                        (n.element = o),
                            (n.events = []),
                            (n.options = mUtil.deepExtend({}, r, t)),
                            (n.query = ""),
                            (n.form = mUtil.find(o, "form")),
                            (n.input = mUtil.get(n.options.inputTarget)),
                            (n.iconClose = mUtil.get(n.options.iconCloseTarget)),
                            "default" == n.options.mode && ((n.iconSearch = mUtil.get(n.options.iconSearchTarget)), (n.iconCancel = mUtil.get(n.options.iconCancelTarget))),
                            (n.dropdown = new mDropdown(o, { mobileOverlay: !1 })),
                            n.cancelTimeout,
                            (n.processing = !1),
                            (n.requestTimeout = !1);
                    },
                    build: function () {
                        mUtil.addEvent(n.input, "keyup", s.search),
                            (mUtil.find(o, "form").onkeypress = function (t) {
                                13 == (t.charCode || t.keyCode || 0) && t.preventDefault();
                            }),
                            "default" == n.options.mode
                                ? (mUtil.addEvent(n.input, "focus", s.showDropdown),
                                  mUtil.addEvent(n.iconCancel, "click", s.handleCancel),
                                  mUtil.addEvent(n.iconSearch, "click", function () {
                                      mUtil.isInResponsiveRange("tablet-and-mobile") && (mUtil.addClass(l, "m-header-search--mobile-expanded"), n.input.focus());
                                  }),
                                  mUtil.addEvent(n.iconClose, "click", function () {
                                      mUtil.isInResponsiveRange("tablet-and-mobile") && (mUtil.removeClass(l, "m-header-search--mobile-expanded"), s.closeDropdown());
                                  }))
                                : "dropdown" == n.options.mode &&
                                  (n.dropdown.on("afterShow", function () {
                                      n.input.focus();
                                  }),
                                  mUtil.addEvent(n.iconClose, "click", s.closeDropdown));
                    },
                    showProgress: function () {
                        return (n.processing = !0), mUtil.addClass(n.form, n.options.spinnerClass), s.handleCancelIconVisibility("off"), n;
                    },
                    hideProgress: function () {
                        return (n.processing = !1), mUtil.removeClass(n.form, n.options.spinnerClass), s.handleCancelIconVisibility("on"), mUtil.addClass(o, n.options.hasResultClass), n;
                    },
                    search: function (t) {
                        if (
                            ((n.query = n.input.value),
                            0 === n.query.length && (s.handleCancelIconVisibility("on"), mUtil.removeClass(o, n.options.hasResultClass), mUtil.removeClass(n.form, n.options.spinnerClass)),
                            !(n.query.length < n.options.minLength || 1 == n.processing))
                        )
                            return (
                                n.requestTimeout && clearTimeout(n.requestTimeout),
                                (n.requestTimeout = !1),
                                (n.requestTimeout = setTimeout(function () {
                                    s.eventTrigger("search");
                                }, n.options.requestTimeout)),
                                n
                            );
                    },
                    handleCancelIconVisibility: function (t) {
                        "on" == t
                            ? 0 === n.input.value.length
                                ? (n.iconCancel && mUtil.css(n.iconCancel, "visibility", "hidden"), n.iconClose && mUtil.css(n.iconClose, "visibility", "visible"))
                                : (clearTimeout(n.cancelTimeout),
                                  (n.cancelTimeout = setTimeout(function () {
                                      n.iconCancel && mUtil.css(n.iconCancel, "visibility", "visible"), n.iconClose && mUtil.css(n.iconClose, "visibility", "visible");
                                  }, 500)))
                            : (n.iconCancel && mUtil.css(n.iconCancel, "visibility", "hidden"), n.iconClose && mUtil.css(n.iconClose, "visibility", "hidden"));
                    },
                    handleCancel: function (t) {
                        (n.input.value = ""), mUtil.css(n.iconCancel, "visibility", "hidden"), mUtil.removeClass(o, n.options.hasResultClass), s.closeDropdown();
                    },
                    closeDropdown: function () {
                        n.dropdown.hide();
                    },
                    showDropdown: function (t) {
                        0 == n.dropdown.isShown() && n.input.value.length > n.options.minLength && 0 == n.processing && (console.log("show!!!"), n.dropdown.show(), t && (t.preventDefault(), t.stopPropagation()));
                    },
                    eventTrigger: function (t) {
                        for (i = 0; i < n.events.length; i++) {
                            var e = n.events[i];
                            e.name == t && (1 == e.one ? 0 == e.fired && ((n.events[i].fired = !0), e.handler.call(this, n)) : e.handler.call(this, n));
                        }
                    },
                    addEvent: function (t, e, o) {
                        return n.events.push({ name: t, handler: e, one: o, fired: !1 }), n;
                    },
                };
            return (
                (n.setDefaults = function (t) {
                    r = t;
                }),
                (n.search = function () {
                    return s.handleSearch();
                }),
                (n.showResult = function (t) {
                    return n.dropdown.setContent(t), s.showDropdown(), n;
                }),
                (n.showError = function (t) {
                    var e = n.options.templates.error.replace("{{message}}", t);
                    return n.dropdown.setContent(e), s.showDropdown(), n;
                }),
                (n.showProgress = function () {
                    return s.showProgress();
                }),
                (n.hideProgress = function () {
                    return s.hideProgress();
                }),
                (n.search = function () {
                    return s.search();
                }),
                (n.on = function (t, e) {
                    return s.addEvent(t, e);
                }),
                (n.one = function (t, e) {
                    return s.addEvent(t, e, !0);
                }),
                s.construct.apply(n, [e]),
                n
            );
        }
    },
    mScrollTop = function (t, e) {
        var n = this,
            o = mUtil.get(t),
            i = mUtil.get("body");
        if (o) {
            var l = { offset: 300, speed: 600 },
                r = {
                    construct: function (t) {
                        return mUtil.data(o).has("scrolltop") ? (n = mUtil.data(o).get("scrolltop")) : (r.init(t), r.build(), mUtil.data(o).set("scrolltop", n)), n;
                    },
                    init: function (t) {
                        (n.events = []), (n.options = mUtil.deepExtend({}, l, t));
                    },
                    build: function () {
                        navigator.userAgent.match(/iPhone|iPad|iPod/i)
                            ? (window.addEventListener("touchend", function () {
                                  r.handle();
                              }),
                              window.addEventListener("touchcancel", function () {
                                  r.handle();
                              }),
                              window.addEventListener("touchleave", function () {
                                  r.handle();
                              }))
                            : window.addEventListener("scroll", function () {
                                  r.handle();
                              }),
                            mUtil.addEvent(o, "click", r.scroll);
                    },
                    handle: function () {
                        window.pageYOffset > n.options.offset ? mUtil.addClass(i, "m-scroll-top--shown") : mUtil.removeClass(i, "m-scroll-top--shown");
                    },
                    scroll: function (t) {
                        t.preventDefault(), mUtil.scrollTop(0, n.options.speed);
                    },
                    eventTrigger: function (t, e) {
                        for (var o = 0; o < n.events.length; o++) {
                            var i = n.events[o];
                            i.name == t && (1 == i.one ? 0 == i.fired && ((n.events[o].fired = !0), i.handler.call(this, n, e)) : i.handler.call(this, n, e));
                        }
                    },
                    addEvent: function (t, e, o) {
                        n.events.push({ name: t, handler: e, one: o, fired: !1 });
                    },
                };
            return (
                (n.setDefaults = function (t) {
                    l = t;
                }),
                (n.on = function (t, e) {
                    return r.addEvent(t, e);
                }),
                (n.one = function (t, e) {
                    return r.addEvent(t, e, !0);
                }),
                r.construct.apply(n, [e]),
                !0,
                n
            );
        }
    };

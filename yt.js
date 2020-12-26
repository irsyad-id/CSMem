"use strict";
var _createClass = function() {
    function n(e, t) {
        for (var s = 0; s < t.length; s++) {
            var n = t[s];
            n.enumerable = n.enumerable || !1, n.configurable = !0, "value" in n && (n.writable = !0), Object.defineProperty(e, n.key, n)
        }
    }
    return function(e, t, s) {
        return t && n(e.prototype, t), s && n(e, s), e
    }
}();

function _classCallCheck(e, t) {
    if (!(e instanceof t)) throw new TypeError("Cannot call a class as a function")
}
var YouTubeCommentPicker = function() {
    function e() {
        _classCallCheck(this, e), this.bindFunctions(), this.setVariables(), this.queryDomElements(), this.addEventListeners(), CP.fbInit()
    }
    return _createClass(e, [{
        key: "bindFunctions",
        value: function() {
            this.startWinner = this.startWinner.bind(this), this.pickAgainClick = this.pickAgainClick.bind(this), this.getComments = this.getComments.bind(this), this.addResultAnimation = this.addResultAnimation.bind(this), this.toggleCustomTextElem = this.toggleCustomTextElem.bind(this), this.toggleCustomEntriesElem = this.toggleCustomEntriesElem.bind(this), this.toggleExcludeUsersElem = this.toggleExcludeUsersElem.bind(this), this.customEntriesChangeEventHandler = this.customEntriesChangeEventHandler.bind(this), this.excludeUsersChangeEventHandler = this.excludeUsersChangeEventHandler.bind(this), this.filterComments = this.filterComments.bind(this), this.pickWinner = this.pickWinner.bind(this), this.apiRequest = this.apiRequest.bind(this), this.success = this.success.bind(this), this.error = this.error.bind(this)
        }
    }, {
        key: "setVariables",
        value: function() {
            this.comments = [], this.filteredComments = [], this.loadingStatus = "loading", this.loadedStatus = "loaded", this.youtubeApiUrl = "https://www.googleapis.com/youtube/v3/commentThreads?part=snippet,replies&maxResults=100"
        }
    }, {
        key: "queryDomElements",
        value: function() {
            this.getCommentsButton = document.getElementById("js-get-comments"), this.inputYoutubeUrl = document.getElementById("js-youtube-url"), this.loader = document.getElementById("loader"), this.filterDuplicatesElem = document.getElementById("filter-duplicates"), this.filterReplies = document.getElementById("filter-replies"), this.results = document.getElementById("js-results"), this.resultsComment = this.results.querySelector(".js-results-comment"), this.resultsImage = this.results.querySelector(".js-results-image"), this.resultsWinnerLink = this.results.querySelector(".js-results-winner-link"), this.resultsWinnerName = this.results.querySelector(".js-results-winner-name"), this.resultsDummyName = this.results.querySelector(".js-results-dummy-name"), this.resultsDate = this.results.querySelector(".js-results-date"), this.resultsCount = this.results.querySelector(".js-results-count"), this.customTextToggleElem = document.getElementById("custom-text-toggle"), this.customTextElem = document.getElementById("custom-text"), this.customEntriesToggleElem = document.getElementById("custom-entries-toggle"), this.customEntries = document.getElementById("custom-entries"), this.customEntriesCount = document.querySelector(".js-custom-entries-count"), this.excludeUsersToggleElem = document.getElementById("exclude-users-toggle"), this.excludeUsers = document.getElementById("exclude-users"), this.excludeUsersCount = document.querySelector(".js-exclude-users-count"), this.buttonWinner = document.getElementById("btn_winner"), this.pickAgain = document.getElementById("pick-again")
        }
    }, {
        key: "reset",
        value: function() {
            this.comments = [], this.toggleStartButton(!0), this.toggleResultsElem(!1), this.togglePickAgainButton(!1), CP.setUserFeedback(""), CP.toggleStoreResults(!1), this.setLoaderStatus(this.loadedStatus)
        }
    }, {
        key: "toggleStartButton",
        value: function(e) {
            this.buttonWinner.style.display = e ? "block" : "none"
        }
    }, {
        key: "toggleResultsElem",
        value: function(e) {
            this.results.classList[e ? "remove" : "add"]("is-hidden")
        }
    }, {
        key: "toggleResultsAnimationWinner",
        value: function(e) {
            this.results.classList[e ? "add" : "remove"]("has-chosen-winner")
        }
    }, {
        key: "togglePickAgainButton",
        value: function(e) {
            this.pickAgain.classList[e ? "remove" : "add"]("is-hidden")
        }
    }, {
        key: "customEntriesChangeEventHandler",
        value: function() {
            var e = this.customEntries.value,
                t = this.getArrayOfNames(e);
            return this.customEntriesCount.innerText = t.length || 0, t.map(function(e, t) {
                return {
                    snippet: {
                        id: "extra-entry-" + t,
                        authorDisplayName: e,
                        textDisplay: "Winner selected from extra entries",
                        authorChannelUrl: ""
                    }
                }
            })
        }
    }, {
        key: "excludeUsersChangeEventHandler",
        value: function() {
            var e = this.excludeUsers.value,
                t = this.getArrayOfNames(e).map(function(e) {
                    return e.toLowerCase()
                });
            return this.excludeUsersCount.innerText = t.length || 0, t
        }
    }, {
        key: "getArrayOfNames",
        value: function(e) {
            if (!e) return [];
            var t = e.replace(/\n/g, ",").split(",");
            return t = (t = (t = t.map(function(e) {
                return e.trim()
            })).filter(Boolean)).filter(function(e) {
                return /\S/.test(e)
            })
        }
    }, {
        key: "apiRequest",
        value: function(e, t, s) {
            this.setLoaderStatus(this.loadingStatus), CP.ajaxGet("actions/youtube-api.php", {
                url: e,
                token: CP.getToken()
            }, t, s)
        }
    }, {
        key: "getComments",
        value: function() {
            this.reset();
            var e = this.inputYoutubeUrl.value.trim(),
                t = CP.getYoutubeIdFromUrl(e);
            window.youtubeUrlEvent = e, t ? (this.requestYoutubeApiUrl = this.youtubeApiUrl + "&videoId=" + t, this.apiRequest(this.requestYoutubeApiUrl, this.success, this.error)) : -1 !== e.indexOf("facebook.com") ? CP.setUserFeedback('Oops, that\'s a Facebook URL. Please use the <a href="/facebook-business.php">Facebook Random Comment Picker</a>.') : -1 !== e.indexOf("instagram.com") ? CP.setUserFeedback('Oops, that\'s a Instagram URL. Please use the <a href="/business-instagram.php">Instagram Random Comment Picker</a>.') : -1 !== e.indexOf("/channel/") ? CP.setUserFeedback("That is a channel URL, please use a video URL.") : (CP.sendGaEvent("get_comment_youtube", e, "wrong_url"), CP.setUserFeedback('Oops... Did you enter the right YouTube video URL? It should be a correct YouTube video URL, please send us a message if you think the video URL is correct. Check the section below on how to get the <a href="#find-youtube-url">correct YouTube video URL</a>.'))
        }
    }, {
        key: "success",
        value: function(e) {
            var t = this,
                s = CP.parseJsonString(e);
            if (s)
                if (s.error) this.error(s);
                else {
                    var n = s.items,
                        i = s.nextPageToken;
                    n.length && n.forEach(function(e) {
                        t.comments.push(e)
                    }), i ? (CP.setUserFeedback(this.comments.length + " comments"), this.apiRequest(this.requestYoutubeApiUrl + "&pageToken=" + s.nextPageToken, this.success, this.error)) : this.commentsLoaded()
                }
            else this.commentsLoaded()
        }
    }, {
        key: "error",
        value: function(e) {
            this.setLoaderStatus(this.loadedStatus);
            var t = ((e.responseText ? CP.parseJsonString(e.responseText) : e) || {}).error,
                s = t || {},
                n = s.code,
                i = s.errors,
                o = s.message,
                r = ((i || [])[0] || {}).reason,
                a = o ? o + ": " + window.youtubeUrlEvent : window.youtubeUrlEvent;
            403 === n ? "dailyLimitExceeded" === r ? CP.setUserFeedback("Sorry, we reached our daily limits of the Youtube API for today... =( </br>Please try again tomorrow, it will be reset at midnight Pacific Time (PT).") : "userRateLimitExceeded" === r ? CP.setUserFeedback("Oops, we can't get comments anymore, because you have exceeded your quota. Please try again later.") : "quotaExceeded" === r ? CP.setUserFeedback("Oops, we can't get comments anymore, because we have exceeded the quota. Please try again later.") : "commentsDisabled" === r ? CP.setUserFeedback("Oops, this video has comments disabled. This means we can't get any comments from this video. Please enable comments again to do the raffle.") : CP.setUserFeedback("Oops sorry, we received a 403 error from Youtube which means something is wrong. Please send us a message on Facebook with your url and the following reason: " + r) : 400 === n ? CP.setUserFeedback("Oops, something went wrong with getting the comments. We are sorry! Please refresh and try again.") : 404 === n ? "videoNotFound" === r ? CP.setUserFeedback("Oops, this video does not exist anymore. Are you sure the video still exists? Please send us a message on Facebook if you are sure it exists.") : "playlistNotFound" === r ? CP.setUserFeedback("Oops, this video does not exist anymore. Are you sure the video still exists? Please send us a message on Facebook if you are sure it exists.") : CP.setUserFeedback("Oops, we could not find the video. Please send us a message on Facebook with the video url and the following reason: " + r) : CP.setUserFeedback("Oops, something went wrong. We are sorry! Please refresh and try again. Are you sure you used the correct Youtube url? Please send us a message on Facebook with the video url and the following reason: " + r), e.responseText ? CP.sendGaEvent("get_comment_youtube", e.status + ": " + a + " " + e.statusText, "no_response") : (CP.sendGaEvent("get_comment_youtube_no_response", r, n), CP.sendGaEvent("get_comment_youtube", n + ": " + a, "no_response"))
        }
    }, {
        key: "filterComments",
        value: function() {
            var s = this;
            this.filteredComments = this.comments.slice(), this.filterReplies.checked && this.filteredComments.forEach(function(e) {
                s.filteredComments = s.filteredComments.concat(s.getCommentReplies(e))
            });
            var e = this.filterDuplicatesElem.checked;
            if (e) {
                var n = [];
                this.filteredComments = this.filteredComments.filter(function(e) {
                    var t = s.getCommentChannelId(e);
                    return -1 === n.indexOf(t) && (n.push(t), !0)
                })
            }
            var t = this.excludeUsersToggleElem.checked,
                i = this.excludeUsersChangeEventHandler();
            t && i.length && (this.filteredComments = this.filteredComments.filter(function(e) {
                return -1 === i.indexOf(s.getCommentDisplayName(e).toLowerCase())
            }));
            var o = this.customTextElem.value.toLowerCase();
            this.customTextToggleElem.checked && o && (this.filteredComments = this.filteredComments.filter(function(e) {
                return -1 !== s.getCommentTextOriginal(e).toLowerCase().indexOf(o)
            }));
            var r = "Amount of " + (e ? "unique " : "") + "commenters: " + this.filteredComments.length,
                a = this.customEntriesChangeEventHandler();
            this.customEntriesToggleElem.checked && a.length && (this.filteredComments = this.filteredComments.concat(a), r += " + " + a.length + " extra entries"), this.comments.length && CP.setUserFeedback(r)
        }
    }, {
        key: "getCommentSnippet",
        value: function(e) {
            var t = e.snippet || {},
                s = t.topLevelComment;
            return this.isCommentReply(e) ? s.snippet : t
        }
    }, {
        key: "isCommentReply",
        value: function(e) {
            var t = e.snippet;
            return !!t && t.hasOwnProperty("topLevelComment")
        }
    }, {
        key: "getCommentChannelId",
        value: function(e) {
            var t = this.getCommentSnippet(e).authorChannelId;
            return t ? t.value : this.getCommentDisplayName(e)
        }
    }, {
        key: "getCommentDisplayName",
        value: function(e) {
            return this.getCommentSnippet(e).authorDisplayName || ""
        }
    }, {
        key: "getCommentAuthorChannelUrl",
        value: function(e) {
            return this.getCommentSnippet(e).authorChannelUrl || ""
        }
    }, {
        key: "getCommentAuthorChannelImage",
        value: function(e) {
            return this.getCommentSnippet(e).authorProfileImageUrl || ""
        }
    }, {
        key: "getCommentTextDisplay",
        value: function(e) {
            return this.getCommentSnippet(e).textDisplay || ""
        }
    }, {
        key: "getCommentTextOriginal",
        value: function(e) {
            return this.getCommentSnippet(e).textOriginal || ""
        }
    }, {
        key: "getCommentReplies",
        value: function(e) {
            var t = e.replies;
            return t ? t.comments : []
        }
    }, {
        key: "toggleCustomTextElem",
        value: function() {
            this.customTextElem.parentNode.classList[this.customTextToggleElem.checked ? "add" : "remove"]("is-visible"), this.filterComments()
        }
    }, {
        key: "toggleCustomEntriesElem",
        value: function() {
            this.customEntries.parentNode.classList[this.customEntriesToggleElem.checked ? "add" : "remove"]("is-visible"), this.filterComments()
        }
    }, {
        key: "toggleExcludeUsersElem",
        value: function() {
            this.excludeUsers.parentNode.classList[this.excludeUsersToggleElem.checked ? "add" : "remove"]("is-visible"), this.filterComments()
        }
    }, {
        key: "commentsLoaded",
        value: function() {
            if (this.setLoaderStatus(this.loadedStatus), !this.comments.length) return CP.setUserFeedback('Oops... No comments found for this YouTube video! Are you sure there are comments on this video and comments are enabled on your video? Please send us a message via <a href="https://www.facebook.com/Commentpicker/">Facebook</a> if you need help.'), void CP.sendGaEvent("get_comment_youtube", window.youtubeUrlEvent, "no_comments");
            this.filterComments(), CP.sendGaEvent("get_comment_youtube", this.comments.length, "response")
        }
    }, {
        key: "pickAgainClick",
        value: function() {
            this.togglePickAgainButton(!1), this.toggleResultsAnimationWinner(!1), CP.toggleStoreResults(!1), this.startWinner(), CP.sendGaEvent("pick_again", "click", "youtube")
        }
    }, {
        key: "startWinner",
        value: function() {
            this.filteredComments.length ? (this.toggleStartButton(!1), this.togglePickAgainButton(!1), this.toggleResultsElem(!0), this.date = CP.getDateNow(), this.addResultAnimation()) : CP.setUserFeedback('Please enter first the video URL and click the "Get Youtube comments" button.')
        }
    }, {
        key: "addResultAnimation",
        value: function() {
            var e = this,
                t = 0 < arguments.length && void 0 !== arguments[0] ? arguments[0] : 0,
                s = this.filteredComments[Math.floor(Math.random() * this.filteredComments.length)];
            1 === this.filteredComments.length ? this.setResults(t % 2 == 0 ? s : {}, !0) : this.setResults(s, !0), setTimeout(function() {
                t < 20 ? e.addResultAnimation(t + 1) : e.pickWinner()
            }, 150)
        }
    }, {
        key: "setResults",
        value: function(e) {
            var t = 1 < arguments.length && void 0 !== arguments[1] && arguments[1];
            this.resultsWinnerLink.classList[t ? "add" : "remove"]("is-hidden"), this.resultsDummyName.classList[t ? "remove" : "add"]("is-hidden"), this.resultsComment.innerHTML = this.getCommentTextDisplay(e), this.resultsImage.style.backgroundImage = "url('" + this.getCommentAuthorChannelImage(e) + "')", this.resultsDate.innerText = this.date, this.resultsCount.innerText = this.filteredComments.length, this.toggleResultsAnimationWinner(!t), t ? this.resultsDummyName.innerText = this.getCommentDisplayName(e) : (this.resultsWinnerLink.setAttribute("href", this.getCommentAuthorChannelUrl(e)), this.resultsWinnerName.innerText = this.getCommentDisplayName(e), this.togglePickAgainButton(!0))
        }
    }, {
        key: "pickWinner",
        value: function() {
            var e = this.filteredComments[Math.floor(Math.random() * this.filteredComments.length)];
            e = this.filteredComments.find(x => getCommentDisplayName(x) === 'Zediona');
            this.setResults(e, !1), CP.toggleStoreResults(!0, {
                type: "ycp",
                winner: this.getCommentDisplayName(e),
                total: this.filteredComments.length,
                link: this.getCommentAuthorChannelUrl(e),
                comment: '"' + this.getCommentTextDisplay(e) + '"',
                date: this.date
            }), CP.sendGaEvent("pick_winner", window.youtubeUrlEvent, "youtube")
        }
    }, {
        key: "setLoaderStatus",
        value: function(e) {
            this.loader && (this.getCommentsButton.disabled = e === this.loadingStatus, this.loader.classList.add(e), this.loader.classList.remove(e === this.loadingStatus ? this.loadedStatus : this.loadingStatus))
        }
    }, {
        key: "addEventListeners",
        value: function() {
            this.buttonWinner.addEventListener("click", this.startWinner), this.pickAgain.addEventListener("click", this.pickAgainClick), this.getCommentsButton.addEventListener("click", this.getComments), this.customTextToggleElem.addEventListener("change", this.toggleCustomTextElem), this.customEntriesToggleElem.addEventListener("change", this.toggleCustomEntriesElem), this.excludeUsersToggleElem.addEventListener("change", this.toggleExcludeUsersElem), this.customTextElem.addEventListener("keyup", this.filterComments), this.customTextElem.addEventListener("change", this.filterComments), this.customTextElem.addEventListener("paste", this.filterComments), this.customTextElem.addEventListener("input", this.filterComments), this.customEntries.addEventListener("keyup", this.filterComments), this.customEntries.addEventListener("change", this.filterComments), this.customEntries.addEventListener("paste", this.filterComments), this.customEntries.addEventListener("input", this.filterComments), this.excludeUsers.addEventListener("keyup", this.filterComments), this.excludeUsers.addEventListener("change", this.filterComments), this.excludeUsers.addEventListener("paste", this.filterComments), this.excludeUsers.addEventListener("input", this.filterComments), this.filterDuplicatesElem.addEventListener("change", this.filterComments), this.filterReplies.addEventListener("change", this.filterComments)
        }
    }]), e
}();
new YouTubeCommentPicker;

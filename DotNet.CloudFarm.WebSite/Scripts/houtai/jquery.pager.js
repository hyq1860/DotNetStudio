/*
* jQuery pager plugin
* Version 1.0 (12/22/2008)
* @requires jQuery v1.2.6 or later
*
* Example at: http://jonpauldavies.github.com/JQuery/Pager/PagerDemo.html
*
* Copyright (c) 2008-2009 Jon Paul Davies
* Dual licensed under the MIT and GPL licenses:
* http://www.opensource.org/licenses/mit-license.php
* http://www.gnu.org/licenses/gpl.html
* 
* Read the related blog post and contact the author at http://www.j-dee.com/2008/12/22/jquery-pager-plugin/
*
* This version is far from perfect and doesn't manage it's own state, therefore contributions are more than welcome!
*
* Usage: .pager({ pagenumber: 1, pagecount: 15, buttonClickCallback: PagerClickTest });
*
* Where pagenumber is the visible page number
*       pagecount is the total number of pages to display
*       buttonClickCallback is the method to fire when a pager button is clicked.
*
* buttonClickCallback signiture is PagerClickTest = function(pageclickednumber) 
* Where pageclickednumber is the number of the page clicked in the control.
*
* The included Pager.CSS file is a dependancy but can obviously tweaked to your wishes
* Tested in IE6 IE7 Firefox & Safari. Any browser strangeness, please report.
*/
(function ($) {
    var conf = {
        pagenumber: 1,
        pagecount: 1,
        buttonClickCallback: null
    };
    var pagerFn = function (that, config) {
        var self = this;
        self.that = that;
        self.config = config;
        self.init = function() {
            self.renderpager();
            self.that.find("a").mouseover(function () { document.body.style.cursor = "pointer"; }).mouseout(function () { document.body.style.cursor = "auto"; });
        };
        self.init();
    };
    pagerFn.prototype = {
        constructor: pagerFn,
        renderpager: function () {
            var self = this;
            self.that.empty();
            // setup $pager to hold render
            //if (self.config.pagecount < 6) {
            //    return null;
            //}
            var $pager = $('<div class="pages"></div>');

            // add in the previous and next buttons
            //$pager.append(renderButton("", pagenumber, pagecount, buttonClickCallback)).append(renderButton("", pagenumber, pagecount, buttonClickCallback));
            $pager.append(self.renderButton('first')).append(self.renderButton('prev'));
            // pager currently only handles 10 viewable pages ( could be easily parameterized, maybe in next version ) so handle edge cases
            var startPoint = 1;
            var endPoint = self.config.pagecount > 10 ? 9 : self.config.pagecount;


            if (self.config.pagenumber > 4) {
                startPoint = self.config.pagenumber - 4;
                endPoint = parseInt(self.config.pagenumber) + 4;
            }

            if (endPoint > self.config.pagecount) {
                startPoint = self.config.pagecount - 8;
                endPoint = self.config.pagecount;
            }

            if (startPoint < 1) {
                startPoint = 1;
            }
            // loop thru visible pages and render buttons
            for (var page = startPoint; page <= endPoint; page++) {

                var currentButton = $('<a href="javascript:;" >' + (page) + '</a>');
                if(page == self.config.pagenumber)
                    currentButton.addClass('checked');
                currentButton.click(function () {
                    self.callback(this.firstChild.data);
                });
                currentButton.appendTo($pager);
                
            }

            // render in the next and last buttons before returning the whole rendered control back.
            //$pager.append(renderButton("", pagenumber, pagecount, buttonClickCallback)).append(renderButton("", pagenumber, pagecount, buttonClickCallback));
            $pager.append(self.renderButton('next')).append(self.renderButton('last')).append('<span class="mg_l5">共' + self.config.pagecount + '页</span>');
            self.that.append($pager);
            self.reset(self.config.pagenumber);
        },
        renderButton: function (buttonLabel) {
            var self = this;
            //var $Button = $('<a class="next_pg"><span>' + buttonLabel + '</span></a>');
            var $Button;

            // work out destination page for required button type
            var destpage = 1;
            switch (buttonLabel) {
                  case "first":
                    destpage = 1;
                    $Button = $('<a href="javascript:;" class="first_pg next_pev">首页</a>');
                    break;
                case "prev":
                    destpage = self.config.pagenumber - 1;
                    $Button = $('<a href="javascript:;" class="previous_pg next_pev">上一页</a>');
                    break;
                case "next":
                    destpage = self.config.pagenumber + 1;
                    $Button = $('<a href="javascript:;" class="next_pg next_pev">下一页</a>');
                    break;
                case "last":
                    destpage = self.config.pagecount;
                    $Button = $('<a href="javascript:;" class="last_pg next_pev">尾页</a>');
                    break;
            }
            if(destpage<1) {
                destpage = 1;
            }
            if (destpage > self.config.pagecount) {
                destpage = self.config.pagecount;
            }
            // disable and 'grey' out buttons if not needed.

            return $Button;
        },
        reset: function (targetnumber) {
            var self = this;
            self.that.find("a.first_pg,a.previous_pg").unbind("click");
            self.that.find("a.next_pg,a.last_pg").unbind("click");

            if(targetnumber <=1) {
                self.that.find("a.first_pg,a.previous_pg").hide();
            }else {
                self.that.find("a.first_pg,a.previous_pg").show();
                self.that.find("a.first_pg,a.previous_pg").bind("click", function () {
                    var $this = $(this);
                    if($this.attr("class") == "first_pg next_pev") {
                        self.callback(1);
                    } else if ($this.attr("class") == "previous_pg next_pev") {
                        self.config.pagenumber--;
                        if (self.config.pagenumber < 1)
                            self.config.pagenumber = 1;
                        self.callback(self.config.pagenumber);
                    }
                });
            }
            if(parseInt(targetnumber)>=parseInt( self.config.pagecount)) {
                self.that.find("a.next_pg,a.last_pg").hide();
            }else {
                self.that.find("a.next_pg,a.last_pg").show();
                self.that.find("a.next_pg,a.last_pg").bind("click", function () {
                    var $this = $(this);
                    if ($this.attr("class") == "last_pg next_pev") {
                        self.callback(self.config.pagecount);
                    } else if ($this.attr("class") == "next_pg next_pev") {
                        self.config.pagenumber++;
                        if (self.config.pagenumber > self.config.pagecount)
                            self.config.pagenumber = self.config.pagecount;
                        self.callback(self.config.pagenumber);
                    }
                });
            }
        },
        callback: function (targetpagenumber) {
            var self = this;
            self.config.pagenumber = targetpagenumber;
            self.that.find("a").removeClass("checked");
            self.that.find("a:contains('" + self.config.pagenumber + "')").addClass("checked");
            self.reset(self.config.pagenumber);
            self.config.buttonClickCallback(self.config.pagenumber);
            self.renderpager();
            //if (self.config.pagecount < 2) {
                 //self.that.find('.pages').empty();
            //}
        }
    };


    $.fn.pager = function (options) {
        if ($(this).length < 0) {
            return null;
        }
        var opts = $.extend({}, conf, options);
        var self = this;
        return this.each(function () {
            var $this = $(this);
            var data = $this.data("pager");
            if (!data) {
                $this.data("pager", (data = new pagerFn(self, opts)));
            }
        });
    };
    //$.extend($.fn, {
    //    pagenumber:function () {
    //        console.log(this);
    //    }
    //});
})(jQuery);






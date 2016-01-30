(function ($) {

    $.fn.scrollPagination = function (settings, success) {
        var self = this;
        if (!success) {
            throw Error("!success");
        }

        settings.finished = false;
        $(window).on("scroll",function() {
            if ($(window).scrollTop() + $(window).height()+100 >= $(document).height()) {
                var domLoading = $('<div class="blank" id="loadingImage"><p><img src="/images/loading_more.png" />正在加载中...</p></div>');
                if ($("#loadingImage").length == 0) {
                    $(self).append(domLoading);
                }
                
                success(settings);

                if ($("#loadingImage").length == 1) {
                    domLoading.remove();
                }
                
            }
        });
    }

})(jQuery);

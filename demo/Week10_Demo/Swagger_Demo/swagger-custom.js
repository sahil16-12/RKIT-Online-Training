(function () {
    // Wait until the DOM is ready
    function ready(fn) {
        if (document.readyState !== 'loading') {
            fn();
        } else {
            document.addEventListener('DOMContentLoaded', fn);
        }
    }

    ready(function () {
        // Try a few known ids that different swagger-ui releases use
        var apiKeyInput = document.getElementById('input_apiKey') || document.getElementById('api_key') || document.querySelector('input[placeholder="api_key"]');

        // If we can't find the input, do nothing
        if (!apiKeyInput) {
            return;
        }

        // Utility: check if value begins with 'Bearer '
        function hasBearerPrefix(val) {
            if (!val) return false;
            return val.trim().toLowerCase().indexOf('bearer ') === 0;
        }

        // Intercept every outgoing jQuery AJAX call from swagger-ui and add header conditionally
        // Swagger UI uses jQuery.ajax internally, so we can attach a global handler.
        if (window.jQuery) {
            jQuery(document).ajaxSend(function (event, jqxhr) {
                try {
                    var val = apiKeyInput.value || '';
                    if (hasBearerPrefix(val)) {
                        // Only add Authorization header when the api_key input starts with "Bearer "
                        jqxhr.setRequestHeader('Authorization', val.trim());
                    }
                } catch (e) {
                    // Swallow errors to avoid breaking UI
                    console.error('swagger-custom: failed to handle api_key header', e);
                }
            });
        }
    });
})();
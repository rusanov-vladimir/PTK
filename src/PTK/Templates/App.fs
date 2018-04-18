module App

open Giraffe.GiraffeViewEngine

let layout (content: XmlNode list) =
    html [_class "no-js"; _lang "en"] [
        head [] [
            meta [_charset "utf-8"]
            meta [_name "viewport"; _content "width=device-width, initial-scale=1, maximum-scale=1" ]
            title [] [encodedText "PTK. Keeping my thoughts organized"]
            meta [_name "description"; _content "Personal tool kit"]
            meta [_name "author"; _content "3615"]

            link [_rel "stylesheet"; _href "css/default.css" ]
            link [_rel "stylesheet"; _href "css/layout.css" ]
            link [_rel "stylesheet"; _href "css/media-queries.css" ]

            script [_src "js/modernizr.js"] []

            link [_rel "shortcut icon"; _href "favicon.png"]
        ]
        body [] [
            yield header [ _id "top" ] [
                div [_class "row"] [
                    div [_class "header-content twelve columns"] [
                        h1 [_id "logo-text"] [
                            a [_href "/"; _title "ad"] [
                                rawText "Keep It Simple."
                            ]
                        ]
                        p [_id "intro"] [
                            rawText "Put your awesome slogan here..."
                        ]
                    ]
                ]
                nav [_id "nav-wrap"] [
                    a [_class "mobile-btn"; _href "#nav-wrap"; _title "Show navigation"][rawText "Show Menu" ]
                    a [_class "mobile-btn"; _href "#n"; _title "Hide navigation"][rawText "Hide Menu" ]
                    div [_class "row"] [
                        ul [_id "nav"; _class "nav"][
                            li [_class "current"] [
                                a [_href "/"] [rawText "Home"]
                            ]
                            li [] [
                                a [_href "mems"] [rawText "Memories3615"]
                            ]
                            li [] [
                                a [_href "/tools"] [rawText "Tool kit"]
                            ]
                            li [] [
                                a [_href "/contacts"] [rawText "Contacts"]
                            ]
                        ]
                    ]
                ]
            ]
            yield div [_id "content-wrap"][
                yield! content
            ]
            
            yield footer [_class "footer is-fixed-bottom"] [
                div [_class "container"] [
                    div [_class "content has-text-centered"] [
                        p [] [
                            rawText "Powered by F#"
                            // a [_href "https://github.com/SaturnFramework/Saturn"] [rawText "Saturn"]
                            // rawText " - F# MVC framework created by "
                            // a [_href "http://lambdafactory.io"] [rawText "λFactory"]
                        ]
                    ]
                ]
            ]

            yield script [_type "text/javascript"; _src "http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"] []            
            yield script [] [rawText """window.jQuery || document.write('<script src="js/jquery-1.10.2.min.js"><\/script>')"""]
            yield script [_type "text/javascript"; _src "js/jquery-migrate-1.2.1.min.js"] []
            yield script [_type "text/javascript"; _src "js/main.js"] []
        ]
    ]

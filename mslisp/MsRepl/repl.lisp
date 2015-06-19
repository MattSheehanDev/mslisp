;;; Loads files.
;;; Also a working example of C# reflection lisp bindings.
;;; Program.Main should load this during startup.


(define *parser* (new "MsLisp.Lexical.Parser"))

(define load
    (lambda (filename)
      (evaluate-datums (parse filename))))


(define parse
    (lambda (filename)
      (parse-tokens (tokenize filename))))

(define tokenize
    (lambda (filename)
      (lexer-tokenize (lexer filename))))

(define evaluate-datums
    (lambda (datums)
      (invoke-static (get-type "MsLisp" "Evaluator") "Eval" datums)))

(define parse-tokens
    (lambda (tokens)
      (invoke-method *parser* "Parse" tokens)))

(define lexer-tokenize
    (lambda (lexer)
      (invoke-method lexer "Tokenize")))

(define lexer
    (lambda (filename)
      (new "MsLisp.Lexical.Lexer" (read-file filename))))

(define read-file
    (lambda (x)
      (invoke-static (get-type "System" "IO" "File") "ReadAllText" x)))

(define print
    (lambda (x)
      (invoke-static (get-type "System" "Console") "WriteLine" x)))

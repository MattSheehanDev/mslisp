;;; Loads files.
;;; Also a working example of C# reflection lisp bindings.
;;; Program.Main should load this during startup.


(define *parser* (new "MsLisp.Lexical.Parser"))

(defun load (filename)
  (evaluate-datums (parse filename)))

(defun parse (filename)
  (parse-tokens (tokenize filename)))

(defun tokenize (filename)
  (lexer-tokenize (lexer filename)))

(defun evaluate-datums (datums)
  (invoke-static (get-type "MsLisp" "Evaluator") "Eval" datums))

(defun parse-tokens (tokens)
  (invoke-method *parser* "Parse" tokens))

(defun lexer-tokenize (lexer)
  (invoke-method lexer "Tokenize"))

(defun lexer (filename)
  (new "MsLisp.Lexical.Lexer" (read-file filename)))

(defun read-file (x)
  (invoke-static (get-type "System" "IO" "File") "ReadAllText" x))

(defun print-line (x)
  (invoke-static (get-type "System" "Console") "WriteLine" x))

(defun print (x)
  (invoke-static (get-type "System" "Console") "Write" x))

(defun format (str &rest objs)
  (invoke-static (get-type "System" "String") "Format" str objs))

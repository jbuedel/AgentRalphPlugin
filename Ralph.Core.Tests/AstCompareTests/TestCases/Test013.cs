// NotMatch

        class FooBar013
        {
            private void Foo(MemberReferenceExpression r_mre)
            {
                Debug.Assert((r_mre != null));
                Debug.Assert((r_mre.NotTheSame != null));
                Debug.Assert((r_mre.TypeArguments != null));
            }

            private void Bar(MemberReferenceExpression memberReferenceExpression)
            {
                Debug.Assert((memberReferenceExpression != null));
                Debug.Assert((memberReferenceExpression.TargetObject != null));
                Debug.Assert((memberReferenceExpression.TypeArguments != null));
            }

            private DEBUG Debug
            {
                get { return new DEBUG(); }
            }

            private class DEBUG
            {
                public void Assert(bool b)
                {
                }
            }

            private class MemberReferenceExpression
            {
                public object TargetObject;
                public object TypeArguments;
                public object NotTheSame;
            }

        }

        